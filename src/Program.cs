using System.CommandLine;
using Microsoft.Extensions.Configuration;

namespace Goodwe.Sems.Sinks.HomeAssistant;

internal class Program
{
    public static async Task<int> Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var username = configuration["sems:username"];
        var password = configuration["sems:password"];
        var plantId = configuration["sems:plant"];

        var sqliteFileOption = new Option<FileInfo?>(
        name: "--sqlite",
        description: "Path to Home Assistant sqlite db file.",
        isDefault: true,
        parseArgument: result =>
        {
            if (result.Tokens.Count == 0)
            {
                return new FileInfo("home-assistant_v2.db");

            }
            string? filePath = result.Tokens.Single().Value;
            if (!File.Exists(filePath))
            {
                result.ErrorMessage = "File does not exist";
                return null;
            }
            else
            {
                return new FileInfo(filePath);
            }
        })
        { IsRequired = true };

        var startAtOption = new Option<DateTimeOffset>(
            aliases: new[] { "--start", "-s" },
            description: "First day of statistics to fetch from Sems, provided in format yyyy-MM-dd.")
        { IsRequired = true };

        var endAtOption = new Option<DateTimeOffset>(
            aliases: new[] { "--end", "-e" },
            description: "Last day of statistics to fetch from Sems, provided in format yyyy-MM-dd.")
        { IsRequired = true };

        var timezoneOption = new Option<TimeSpan>(
            aliases: new[] { "--timezone", "-z" },
            description: "Your Sems & Home Assistant Timezone offset in format ±hh:mm.")
        { IsRequired = true };

        var semsUsernameOption = new Option<string>(
            aliases: new[] { "--username", "-u" },
            description: "Sems portal username.",
            getDefaultValue: () => username)
        { IsRequired = true };

        var semsPasswordOption = new Option<string>(
            aliases: new[] { "--password", "-p" },
            description: "Sems portal password.",
            getDefaultValue: () => password)
        { IsRequired = true };

        var semsPlantIdOption = new Option<string>(
            aliases: new[] { "--plant", "-i" },
            description: "Sems plant id.",
            getDefaultValue: () => plantId)
        { IsRequired = true };

        var rootCommand = new RootCommand("Import Goodwe Sems HomeKit data into Home Assistant");

        var recordCommand = new Command("record", "Record Sems statistics in Home Assistant.")
        {
            sqliteFileOption,
            semsUsernameOption,
            semsPasswordOption,
            semsPlantIdOption,
            startAtOption,
            endAtOption,
            timezoneOption,
        };
        rootCommand.AddCommand(recordCommand);

        var getSemsSensorsCommand = new Command("sensors", "Get Sems sensors.")
        {
            semsUsernameOption,
            semsPasswordOption,
            semsPlantIdOption
        };

        var semsCommand = new Command("sems", "Sems utilities.");
        semsCommand.AddCommand(getSemsSensorsCommand);
        rootCommand.AddCommand(semsCommand);

        var getHassSensorsCommand = new Command("sensors", "Get Home Assistant sensors.")
        {
            sqliteFileOption
        };

        var hassCommand = new Command("hass", "Home Assistatnt utilities.");
        hassCommand.AddCommand(getHassSensorsCommand);
        rootCommand.AddCommand(hassCommand);

        recordCommand.SetHandler(async (dbfile, semsUsername, semsPassword, semsPlantId, startAt, endAt, timezoneOffset) =>
        {
            var recorder = new Recorder();
            await recorder.Record(dbfile!, semsUsername, semsPassword, semsPlantId, startAt.ToOffset(timezoneOffset), endAt.ToOffset(timezoneOffset));
        },
        sqliteFileOption, semsUsernameOption, semsPasswordOption, semsPlantIdOption, startAtOption, endAtOption, timezoneOption);

        getSemsSensorsCommand.SetHandler(async (semsUsername, semsPassword, semsPlantId) =>
        {
            var finder = new Finder();
            await finder.FindSemsSensors(semsUsername, semsPassword, semsPlantId);
        },
        semsUsernameOption, semsPasswordOption, semsPlantIdOption);

        getHassSensorsCommand.SetHandler(async (dbFile) =>
        {
            var finder = new Finder();
            await finder.FindHassSensors(dbFile!);
        },
        sqliteFileOption);

        return await rootCommand.InvokeAsync(args);
    }
}