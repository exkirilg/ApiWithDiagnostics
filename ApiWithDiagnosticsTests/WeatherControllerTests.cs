using ApiWithDiagnostics;
using ApiWithDiagnostics.Controllers;
using ApiWithDiagnostics.Models;
using ApiWithDiagnostics.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiWithDiagnosticsTests;

public class WeatherControllerTests
{
    [Fact]
    public async Task ReturnsExpectedResult()
    {
        float latitude = 34.21f;
        float longitude = 23.17f;
        DateOnly date = DateOnly.Parse("2023-01-01");

        var exp = new DailyWeather()
        {
            Latitude = latitude,
            Longitude = longitude,
            Hourly = new Hourly()
            {
                Time = new List<string>() { date.ToString() },
                Weathercode = new List<int>() { 0 }
            }
        };

        var httpRequestsMock = new Mock<IHttpRequests>();
        httpRequestsMock
            .Setup(x => x.Fetch<DailyWeather>(HttpClients.ArchiveWeatherHttpClientName, It.IsAny<string>()))
            .ReturnsAsync(exp);

        var sut = new WeatherController(httpRequestsMock.Object);

        var result = await sut.GetHourlyWeathercodesForDate(latitude, longitude, date);

        var okObjResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjResult.StatusCode);
        Assert.Equal(exp, okObjResult.Value);
    }
}
