using System.Text.Json;
using System.Text.Json.Nodes;

namespace Goodwe.Sems;

public class SemsApi
{
    private readonly string username;
    private readonly string password;
    private readonly string plantId;
    private string token = "";
    private string api = "";
    private readonly bool expireCache;

    public SemsApi(string username, string password, string plantId, bool expireCache = false)
    {
        this.username = username;
        this.password = password;
        this.plantId = plantId;
        this.expireCache = expireCache;
    }

    public async Task<DayExtract> GetStatsByDateAsync(DateTimeOffset date)
    {
        var jsonData = await GetWithCacheAsync($"dump/{date:yyyy-MM-dd}-chart-data.json", () => GetChartByPlantAsync(date));
        var data = JsonSerializer.Deserialize<ChartData.Data>(jsonData);
        if (null == data) throw new NullReferenceException("Could not deserialize Sems chart data.");
        var importExport = data.lines.Single(x => x.label.Contains("Meter", StringComparison.CurrentCultureIgnoreCase)).xy;
        var load = data.lines.Single(x => x.label.Contains("Load", StringComparison.CurrentCultureIgnoreCase)).xy;

        return new DayExtract
        {
            Import = importExport.ToDictionary(
                item => item.x,
                item => (item.y < 0 ? item.y : 0) * -1
            ),
            Export = importExport.ToDictionary(
                item => item.x,
                item => item.y > 0 ? item.y : 0
            ),
            Load = load.ToDictionary(
                item => item.x,
                item => item.y
            )
        };
    }

    public async Task<string[]> GetSensorsAsync()
    {
        var data = await GetWithCacheAsync("dump/details.json", GetMonitorDetailByPowerstationIdAsync);
        var rootNode = JsonNode.Parse(data);

        try
        {
            var homeKitSn = rootNode!["homKit"]!["sn"]!.GetValue<string>();
            if (string.IsNullOrEmpty(homeKitSn))
                throw new ApplicationException("root.homKit.hn was empty.");

            return new[]
            {
                // Default sensor names from Goodwe Sems Home Assistant plugin/hacs
                $"HomeKit_{homeKitSn}_Import",
                $"HomeKit_{homeKitSn}_Export",
            };
        }
        catch (System.Exception ex)
        {
            throw new ApplicationException("Cound't find homeKit serial number in Sems.", ex);
        }
    }

    private async Task<string> GetWithCacheAsync(string fileName, Func<Task<string>> get, bool forceLogin = false)
    {
        if (!expireCache && File.Exists(fileName))
        {
            return await File.ReadAllTextAsync(fileName);
        }
        
        if (string.IsNullOrEmpty(token))
        {
            await LoginAsync();
        }
        var data = await get();
        Directory.CreateDirectory(fileName);
        await File.WriteAllTextAsync(fileName, data);
        return data;
    }

    private async Task LoginAsync()
    {
        if (!string.IsNullOrEmpty(token)) return;

        Console.WriteLine("> [DEBUG]: Logging into Sems...");
        var loginHttpClient = new HttpClient();
        loginHttpClient.BaseAddress = new Uri("https://www.semsportal.com");

        var login_data = JsonSerializer.Serialize(new
        {
            account = username,
            pwd = password
        });

        var loginRequest = new HttpRequestMessage(HttpMethod.Post, "api/v2/Common/CrossLogin");
        loginRequest.Headers.Add("token", "{\"version\":\"\",\"client\":\"ios\",\"language\":\"en\"}");
        loginRequest.Headers.Add("Accept", "application/json");
        loginRequest.Content = new StringContent(login_data, System.Text.Encoding.UTF8, mediaType: "application/json");
        var loginResponse = await loginHttpClient.SendAsync(loginRequest);
        var responseData = await loginResponse.Content.ReadAsStringAsync();
        var responseObject = JsonNode.Parse(responseData);
        var loginToken = responseObject!["data"];
        this.token = loginToken.ToJsonString();
        this.api = responseObject!["api"].GetValue<string>();
    }

    private async Task<string> GetChartByPlantAsync(DateTimeOffset date)
    {
        var fetchHttpClient = new HttpClient();
        fetchHttpClient.BaseAddress = new Uri(this.api);
        var fetch_data = JsonSerializer.Serialize(new
        {
            id = plantId,
            date = date.ToString("yyyy-MM-dd"),
            range = 2,
            chartIndexId = "1",
            isDetailFull = ""
        });

        var fetchRequest = new HttpRequestMessage(HttpMethod.Post, "v2/Charts/GetChartByPlant");
        fetchRequest.Headers.Add("token", token);
        fetchRequest.Headers.Add("Accept", "application/json");
        fetchRequest.Content = new StringContent(fetch_data, System.Text.Encoding.UTF8, mediaType: "application/json");
        var fetchResponse = await fetchHttpClient.SendAsync(fetchRequest);
        var fetchedData = await fetchResponse.Content.ReadAsStringAsync();
        var fetchedObject = JsonNode.Parse(fetchedData);
        return fetchedObject!["data"].ToJsonString();
    }

    private async Task<string> GetMonitorDetailByPowerstationIdAsync()
    {
        var fetchHttpClient = new HttpClient();
        fetchHttpClient.BaseAddress = new Uri(this.api);
        var fetch_data = JsonSerializer.Serialize(new
        {
            powerStationId = plantId
        });

        var fetchRequest = new HttpRequestMessage(HttpMethod.Post, "v2/PowerStation/GetMonitorDetailByPowerstationId");
        fetchRequest.Headers.Add("token", token);
        fetchRequest.Headers.Add("Accept", "application/json");
        fetchRequest.Content = new StringContent(fetch_data, System.Text.Encoding.UTF8, mediaType: "application/json");
        var fetchResponse = await fetchHttpClient.SendAsync(fetchRequest);
        var fetchedData = await fetchResponse.Content.ReadAsStringAsync();
        var fetchedObject = JsonNode.Parse(fetchedData);
        return fetchedObject!["data"].ToJsonString();
    }
}