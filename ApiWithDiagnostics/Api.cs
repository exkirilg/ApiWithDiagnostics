using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiWithDiagnostics;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("current-weather/{latitude}-{longitude}", GetCurrentWeather);
        app.MapGet("random-cats-facts", GetRandomCatsFact);
        app.MapGet("postman-echo", GetPostmanEcho);
    }

    private static async Task<IResult> GetCurrentWeather(
        [FromServices] IHttpClientFactory httpClientFactory, [FromServices] ILoggerFactory loggerFactory,
        float latitude, float longitude)
    {
        var httpClient = httpClientFactory.CreateClient(HttpClients.WeatherHttpClientName);
        var logger = loggerFactory.CreateLogger($"{typeof(HttpClient)}.{HttpClients.WeatherHttpClientName}");

        string requestUri = $"forecast?latitude={latitude:0.0000}&longitude={longitude:0.0000}&current_weather=true";

        return await ProcessHttpRequest(httpClient, requestUri, logger);
    }

    private static async Task<IResult> GetRandomCatsFact(
        [FromServices] IHttpClientFactory httpClientFactory, [FromServices] ILoggerFactory loggerFactory,
        int amount = 1)
    {
        var httpClient = httpClientFactory.CreateClient(HttpClients.CatsFactsHttpClientName);
        var logger = loggerFactory.CreateLogger($"{typeof(HttpClient)}.{HttpClients.CatsFactsHttpClientName}");

        string requestUri = $"facts/random?animal_type=cat&amount={amount}";

        return await ProcessHttpRequest(httpClient, requestUri, logger);
    }

    private static async Task<IResult> GetPostmanEcho(
        [FromServices] IHttpClientFactory httpClientFactory, [FromServices] ILoggerFactory loggerFactory)
    {
        var httpClient = httpClientFactory.CreateClient(HttpClients.PostmanEchoHttpClientName);
        var logger = loggerFactory.CreateLogger($"{typeof(HttpClient)}.{HttpClients.PostmanEchoHttpClientName}");

        return await ProcessHttpRequest(httpClient, "get?foo=bar", logger);
    }

    private static async Task<IResult> ProcessHttpRequest(HttpClient httpClient, string requestUri, ILogger logger)
    {
        try
        {
            string data = await FetchRequestedDataAsString(httpClient, requestUri);
            return Results.Ok(data);
        }
        catch (HttpRequestException)
        {
            logger.LogWarning("Unable to fetch data: {RequestUri}", $"{httpClient.BaseAddress}{requestUri}");
            return Results.Problem(detail: "Unable to fetch data", statusCode: 400);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unhandled exception while fetching data: {RequestUri}", $"{httpClient.BaseAddress}{requestUri}");
            return Results.Problem(detail: "Unable to fetch data", statusCode: 500);
        }
    }

    private static async Task<string> FetchRequestedDataAsString(HttpClient httpClient, string requestUri)
    {
        var result = await httpClient.GetAsync(requestUri);
        result.EnsureSuccessStatusCode();
        return await result.Content.ReadAsStringAsync();
    }
}
