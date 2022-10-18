using Hass;

namespace Goodwe.Sems.Sinks.HomeAssistant;

public class Finder
{
    public async Task FindSemsSensors(string semsUsername, string semsPassword, string plantId)
    {
        var semsApi = new SemsApi(semsUsername, semsPassword, plantId);
        var sensors = await semsApi.GetSensorsAsync();
        System.Console.WriteLine($"[1/1] Finding Sems sensors...");
        foreach (var sensor in sensors)
        {
            System.Console.WriteLine($"> [INFO]: found {sensor}.");
        }
        System.Console.WriteLine("Done");
    }

    public async Task FindHassSensors(FileInfo dbfile)
    {
        var hassHelper = new HassHelper(dbfile);
        System.Console.WriteLine($"[1/1] Finding Home Assistant sensors...");
        var sensors = await hassHelper.GetSemsSensorsAsync();
        foreach (var sensor in sensors)
        {
            System.Console.WriteLine($"> [INFO]: found {sensor.id}:{sensor.statistic_id}");
        }
        System.Console.WriteLine("Done");
    }
}
