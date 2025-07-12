namespace SurfForecastApi.Models;

public class MaritimeResponse
{
    public double WaveHeight { get; set; }
    public string WindWaveDirection { get; set; } = string.Empty;
    public double SwellPeriod { get; set; }
}
