using ApiWithDiagnostics.Models;
using ApiWithDiagnostics.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithDiagnostics.Controllers;

[Route("[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly IHttpRequests _httpRequests;

    public WeatherController(IHttpRequests httpRequests)
    {
        _httpRequests = httpRequests;
    }

    [HttpGet("hourly-weathercodes")]
    public async Task<IActionResult> GetHourlyWeathercodesForDate(
        [FromQuery] float latitude, [FromQuery] float longitude, [FromQuery] DateOnly date)
    {
        string requestUri = $"era5?latitude={latitude:0.0000}&longitude={longitude:0.0000}&start_date={date:yyyy-MM-dd}&end_date={date.AddDays(1):yyyy-MM-dd}&hourly=weathercode";

        var result = await _httpRequests.Fetch<DailyWeather>(HttpClients.ArchiveWeatherHttpClientName, requestUri);

        return Ok(result);
    }
}
