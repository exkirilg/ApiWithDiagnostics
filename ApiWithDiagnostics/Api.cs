using ApiWithDiagnostics.Diagnostics;
using ApiWithDiagnostics.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApiWithDiagnostics;

public static class Api
{
    private static Stopwatch _stopwatch = new();

    public static void ConfigureApi(this WebApplication app)
    {
        app
            .MapGet("current-weather/{latitude}-{longitude}", GetCurrentWeather)
            .AddEndpointFilter(async (context, next) =>
            {
                return await EndpointFilterHandler(context, next);
            });
        
        app
            .MapGet("random-cats-facts", GetRandomCatsFact)
            .AddEndpointFilter(async (context, next) =>
            {
                return await EndpointFilterHandler(context, next);
            });

        app
            .MapGet("postman-echo", GetPostmanEcho)
            .AddEndpointFilter(async (context, next) =>
            {
                return await EndpointFilterHandler(context, next);
            });
    }

    private static async Task<IResult> GetCurrentWeather(
        [FromServices] IHttpRequests httpRequests, [FromServices] ILoggerFactory loggerFactory,
        float latitude, float longitude)
    {
        var logger = loggerFactory.CreateLogger($"{typeof(HttpClient)}.{HttpClients.WeatherHttpClientName}");

        string requestUri = $"forecast?latitude={latitude:0.0000}&longitude={longitude:0.0000}&current_weather=true";

        return await ProcessHttpRequestWithStringResult(httpRequests, HttpClients.WeatherHttpClientName, requestUri, logger);
    }

    private static async Task<IResult> GetRandomCatsFact(
        [FromServices] IHttpRequests httpRequests, [FromServices] ILoggerFactory loggerFactory,
        int amount = 1)
    {
        var logger = loggerFactory.CreateLogger($"{typeof(HttpClient)}.{HttpClients.CatsFactsHttpClientName}");

        string requestUri = $"facts/random?animal_type=cat&amount={amount}";

        return await ProcessHttpRequestWithStringResult(httpRequests, HttpClients.CatsFactsHttpClientName, requestUri, logger);
    }

    private static async Task<IResult> GetPostmanEcho(
        [FromServices] IHttpRequests httpRequests, [FromServices] ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger($"{typeof(HttpClient)}.{HttpClients.PostmanEchoHttpClientName}");

        return await ProcessHttpRequestWithStringResult(httpRequests, HttpClients.PostmanEchoHttpClientName, "get?foo=bar", logger);
    }

    private static async Task<IResult> ProcessHttpRequestWithStringResult(
        IHttpRequests httpRequests, string httpClientName, string requestUri, ILogger logger)
    {
        try
        {
            string data = await httpRequests.FetchAsString(httpClientName, requestUri);
            return Results.Ok(data);
        }
        catch (HttpRequestException)
        {
            logger.LogWarning("Unable to fetch data: {HttpClient} - {RequestUri}", httpClientName, requestUri);
            return Results.Problem(detail: "Unable to fetch data", statusCode: 400);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unhandled exception while fetching data: {HttpClient} - {RequestUri}", httpClientName, requestUri);
            return Results.Problem(detail: "Unable to fetch data", statusCode: 500);
        }
    }

    private static async Task<object?> EndpointFilterHandler(
        EndpointFilterInvocationContext efContext, EndpointFilterDelegate next)
    {
        _stopwatch.Start();

        var result = await next(efContext);

        _stopwatch.Stop();

        EventCounterSource.Log.RequestTime(
            efContext.HttpContext.Request.GetDisplayUrl(),
            _stopwatch.ElapsedMilliseconds
        );

        return result;
    }
}
