namespace ApiWithDiagnostics.Models;

public class DailyWeather
{
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public float Elevation { get; set; }
    public Hourly Hourly { get; set; } = new();
}

public class Hourly
{
    public IEnumerable<string> Time { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<int> Weathercode { get; set; } = Enumerable.Empty<int>();
}
