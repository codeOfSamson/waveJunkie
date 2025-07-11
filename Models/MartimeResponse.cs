namespace SurfForecastApi.Models;

public class MaritimeResponse
{
    public double WaveHeight { get; set; }
    public double WindSpeed { get; set; }
    public string WindDirection { get; set; } = string.Empty;
    public double SwellPeriod { get; set; }
}
