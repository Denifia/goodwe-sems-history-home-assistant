using Dapper;
using Goodwe.Sems;
using Hass.Db;
using Microsoft.Data.Sqlite;
using Z.Dapper.Plus;

namespace Hass;

public class HassHelper
{
    private readonly FileInfo dbfile;
    private string connectionString => $"Data Source={this.dbfile.FullName}";

    public HassHelper(FileInfo dbfile)
    {
        this.dbfile = dbfile;
    }

    public async Task RecalculateSumsAsync(Hass.Db.metadata[] sensors, TimeSpan timezone)
    {
        foreach (var sensor in sensors)
        {
            await RecalculateSumsAsync(sensor, timezone);
        }
    }

    private async Task RecalculateSumsAsync(Hass.Db.metadata sensor, TimeSpan timezone)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            await connection.OpenAsync();

            var offset = 0;
            float previousSum = 0;
            decimal previousState = 0;
            DateTime? tomorrow = null;
            var lastStatOfEachHour = new Dictionary<DateTime, float>();
            while (true)
            {
                var shortTermStats = await connection.QueryAsync<statistics_short_term>("SELECT * FROM statistics_short_term where metadata_id = @metadata_id order by start asc limit 100 offset @offset", new { metadata_id = sensor.id, offset });
                if (!shortTermStats.Any()) break;
                var trans = await connection.BeginTransactionAsync();
                foreach (var stat in shortTermStats)
                {
                    if (tomorrow == null || stat.start >= tomorrow.Value)
                    {
                        stat.sum = previousSum + (float)(stat.state - 0); // reset because it's a new day
                    }
                    else
                    {
                        stat.sum = previousSum + (float)(stat.state - previousState);
                    }
                    
                    previousSum = stat.sum;
                    previousState = stat.state;

                    var now = new DateTimeOffset(stat.start, TimeSpan.Zero)
                        .ToOffset(timezone);
                    tomorrow = now
                        .Subtract(now.TimeOfDay) // start of today
                        .AddDays(1) // tomorrow in your timezone
                        .UtcDateTime; // back into utc

                    if (stat.start.Minute == 55)
                    {
                        lastStatOfEachHour.Add(stat.start.AddMinutes(-55), stat.sum);
                    }
                }
                trans.BulkUpdate(shortTermStats);
                await trans.CommitAsync();
                await trans.DisposeAsync();
                offset += 100;
            }
            
            offset = 0;
            while (true)
            {
                var longTermStats = await connection.QueryAsync<statistics>("SELECT * FROM statistics where metadata_id = @metadata_id order by start asc limit 100 offset @offset", new { metadata_id = sensor.id, offset });
                if (!longTermStats.Any()) break;
                var trans = await connection.BeginTransactionAsync();
                foreach (var stat in longTermStats)
                {
                    if (!lastStatOfEachHour.ContainsKey(stat.start)) 
                        continue;
                    // todo: should I throw here?
                    // throw new KeyNotFoundException($"Couldn't find matching short term stat for metadata_id {stat.metadata_id} with start {stat.start:yyyy-MM-dd HH:mm}.");
                    stat.sum = lastStatOfEachHour[stat.start];
                }
                trans.BulkUpdate(longTermStats);
                await trans.CommitAsync();
                await trans.DisposeAsync();
                offset += 100;
            }
        }
        System.Console.WriteLine($"> [INFO]: Recalculated {sensor.statistic_id}");
    }

    public async Task RecordHistoricalStatsAsync(FullExtract extract, Hass.Db.metadata[] sensors)
    {
        var importSensor = sensors.Single(x => x.statistic_id.EndsWith("import"));
        System.Console.WriteLine($"> [INFO]: Importing to {importSensor.statistic_id}...");
        foreach (var data in extract.Import)
        {
            await RecordHistoricalStatsAsync(data.Key, data.Value, importSensor);
        }

        var exportSensor = sensors.Single(x => x.statistic_id.EndsWith("export"));
        System.Console.WriteLine($"> [INFO]: Importing to {importSensor.statistic_id}...");
        foreach (var data in extract.Export)
        {   
            await RecordHistoricalStatsAsync(data.Key, data.Value, exportSensor);
        }
    }

    private async Task RecordHistoricalStatsAsync(DateTimeOffset date, Dictionary<string, double> data, Hass.Db.metadata sensor)
    {
        var metadata_id = sensor.id;
        var shortTermStats = new List<statistics_short_term>();
        var longTermStats = new List<statistics>();
        decimal last_state = 0;

        foreach (var item in data)
        {
            var hour = int.Parse(item.Key.Split(":")[0]); // 24 hour format
            var minute = int.Parse(item.Key.Split(":")[1]);

            var state = (decimal)Math.Round(item.Value / 12 / 1000, 2, MidpointRounding.AwayFromZero);
            var shortTermStat = new statistics_short_term
            {
                created = DateTime.UtcNow,
                start = new DateTimeOffset(date.Year, date.Month, date.Day, hour, minute, 0, 0, date.Offset).UtcDateTime,
                state = state + last_state,
                sum = 0, // calc this later
                metadata_id = metadata_id
            };

            shortTermStats.Add(shortTermStat);
            last_state = shortTermStat.state;

            if (minute == 55)
            {
                var longTermStat = new statistics
                {
                    created = shortTermStat.created,
                    start = new DateTimeOffset(date.Year, date.Month, date.Day, hour, 0, 0, 0, date.Offset).UtcDateTime,
                    state = shortTermStat.state,
                    sum = 0, // calc this later
                    metadata_id = metadata_id
                };
                longTermStats.Add(longTermStat);
            }
        }

        DapperPlusManager.Entity<statistics>().Identity(x => x.id);
        DapperPlusManager.Entity<statistics_short_term>().Identity(x => x.id);
        using (var connection = new SqliteConnection(connectionString))
        {
            await connection.OpenAsync();
            var trans = await connection.BeginTransactionAsync();
            trans.BulkInsert(shortTermStats);
            trans.BulkInsert(longTermStats);
            await trans.CommitAsync();
        }
        System.Console.WriteLine($"> [INFO]: Imported {date:yyyy-MM-dd}");
    }

    public async Task ClearStatsPriorTooAsync(DateTimeOffset before, Hass.Db.metadata[] sensors)
    {
        foreach (var sensor in sensors)
        {
            await ClearStatsPriorTooAsync(before, sensor.id);
        }
    }

    private async Task ClearStatsPriorTooAsync(DateTimeOffset before, int metadata_id)
    {
        var lastGoodDate = before.AddMinutes(1).ToString("yyyy-MM-dd HH:mm zzz");
        using (var connection = new SqliteConnection(connectionString))
        {
            await connection.ExecuteAsync("delete from statistics where metadata_id = @metadata_id and datetime(start) < datetime(@lastGoodDate)", new { metadata_id, lastGoodDate });
            await connection.ExecuteAsync("delete from statistics_short_term where metadata_id = @metadata_id and datetime(start) < datetime(@lastGoodDate)", new { metadata_id, lastGoodDate });
        }
    }

    public async Task<int> GetMetadataIdAsync(string sensor)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            return await connection.ExecuteScalarAsync<int>("SELECT id FROM statistics_meta where statistic_id = @sensor", new { sensor });
        }
    }

    public async Task<IEnumerable<metadata>> GetSemsSensorsAsync()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var homekitSensors = await connection.QueryAsync<metadata>("SELECT id, statistic_id FROM statistics_meta where statistic_id like 'sensor.homekit%'");
            // todo: add support for inverter data later
            //var inverterSensors = await connection.QueryAsync<metadata>("SELECT id, statistic_id FROM statistics_meta where statistic_id like 'sensor.inverter%'");
            return homekitSensors;
        }
    }

    public async Task LogSemsSensorStatCount(Hass.Db.metadata sensor)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var longTermStatsCount = await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM statistics where metadata_id = @id", new { id = sensor.id });
            var shortTermStatsCount = await connection.ExecuteScalarAsync<int>("SELECT count(*) FROM statistics_short_term where metadata_id = @id", new { sensor.id });
            System.Console.WriteLine($"{sensor.statistic_id} has {longTermStatsCount} long and {shortTermStatsCount} short term stats.");
        }
    }
}
