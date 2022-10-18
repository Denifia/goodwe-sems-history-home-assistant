using Hass;

namespace Goodwe.Sems.Sinks.HomeAssistant
{
    public class Recorder
    {
        public async Task Record(
            FileInfo dbfile,
            string semsUsername,
            string semsPassword,
            string plantId,
            DateTimeOffset startDate,
            DateTimeOffset endDate)
        {
            System.Console.WriteLine($"[1/5] Finding sensors...");
            var semsApi = new SemsApi(semsUsername, semsPassword, plantId);
            var semsSensors = await semsApi.GetSensorsAsync();

            var hassHelper = new HassHelper(dbfile);
            var hassSensors = await hassHelper.GetSemsSensorsAsync();

            var matchingSensors = hassSensors
                .ToArray()
                .Where(x => semsSensors.Any(y => string.Equals(x.statistic_id, $"sensor.{y}", StringComparison.CurrentCultureIgnoreCase)))
                .ToArray();

            foreach (var item in matchingSensors)
            {
                System.Console.WriteLine($"> [INFO]: Found {item.statistic_id}");
            }

            System.Console.WriteLine($"[2/5] Downloading Goodwe Sems HomeKit data from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}...");
            var fullExtract = new FullExtract();
            foreach (DateTimeOffset day in EachDay(startDate, endDate))
            {
                var dayExtract = await semsApi.GetStatsByDateAsync(day);    
                fullExtract.Import.Add(day, dayExtract.Import);
                fullExtract.Export.Add(day, dayExtract.Export);
                fullExtract.Load.Add(day, dayExtract.Load);
                System.Console.WriteLine($"> [INFO]: Downloaded {day:yyyy-MM-dd}");
            }

            System.Console.WriteLine($"[3/5] Removing statistics on and before {endDate:yyyy-MM-dd}...");
            await hassHelper.ClearStatsPriorTooAsync(endDate.AddDays(1), matchingSensors);

            System.Console.WriteLine("[4/5] Importing statistics...");
            await hassHelper.RecordHistoricalStatsAsync(fullExtract, matchingSensors);

            System.Console.WriteLine("[5/5] Recalculating statistic sums...");
            await hassHelper.RecalculateSumsAsync(matchingSensors, startDate.Offset);

            System.Console.WriteLine("Done");
        }

        private IEnumerable<DateTimeOffset> EachDay(DateTimeOffset from, DateTimeOffset thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}