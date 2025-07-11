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
        var url = $"{lat},{lon}?for=now&to=now&client_id={_clientId}&client_secret={_clientSecret}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var nowData = doc.RootElement.GetProperty("hours")[0];

        return new MaritimeResponse
        {
            WaveHeight = nowData.GetProperty("waveHeight").GetDouble(),
            WindSpeed = nowData.GetProperty("windSpeed").GetDouble(),
            WindDirection = nowData.GetProperty("windDirection").GetString() ?? "N/A",
            SwellPeriod = nowData.GetProperty("swellPeriod").GetDouble()
        };
    }
}

public class XweatherSettings
{
    public string BaseUrl { get; set; } = "";
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
}
