using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SurfForecastApi.Models;

namespace SurfForecastApi.Services;

public class XweatherClient
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public XweatherClient(HttpClient httpClient, IOptions<XweatherSettings> options)
    {
        _httpClient = httpClient;
        _clientId = options.Value.ClientId;
        _clientSecret = options.Value.ClientSecret;
    }

    public async Task<MaritimeResponse?> GetMaritimeDataAsync(double lat, double lon)
    {
        var url = $"/maritime/{lat},{lon}?for=now&to=now&client_id={_clientId}&client_secret={_clientSecret}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"DEBUG: Response JSON: {json}");

        using var doc = JsonDocument.Parse(json);

        // The API returns a response array with periods
        var responseArray = doc.RootElement.GetProperty("response");
        if (responseArray.GetArrayLength() == 0) return null;

        var firstLocation = responseArray[0];
        var periods = firstLocation.GetProperty("periods");
        if (periods.GetArrayLength() == 0) return null;

        var nowData = periods[0];
        Console.WriteLine($"DEBUG: Processing period data: {nowData}");

        return new MaritimeResponse
        {
            WaveHeight = nowData.GetProperty("significantWaveHeightM").GetDouble(),
            WindWaveDirection = nowData.GetProperty("windWaveDir").GetString() ?? "N/A",
            SwellPeriod = nowData.GetProperty("primarySwellPeriod").GetDouble()
        };
    }
}

public class XweatherSettings
{
    public string BaseUrl { get; set; } = "";
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
}
