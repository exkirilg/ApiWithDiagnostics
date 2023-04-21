using System.Diagnostics;

namespace ApiWithDiagnostics.Middlewares;

public class MetricsMiddleware
{
    private readonly RequestDelegate _next;

    public MetricsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<MetricsMiddleware> logger)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint is null)
            await ProcessRequest(context);
        else
            await ProcessRequestWithMetricsLogged(context, logger);
    }

    private async Task ProcessRequest(HttpContext context)
    {
        await _next(context);
    }

    private async Task ProcessRequestWithMetricsLogged(HttpContext context, ILogger _logger)
    {
        Stopwatch sw = Stopwatch.StartNew();
        
        await _next(context);
        
        _logger.LogInformation(
            "Request processed in {ElapsedMilliseconds} ms",
            sw.Elapsed.TotalMilliseconds.ToString("0.00")
        );
    }
}

public static class MetricsMiddlewareExtensions
{
    public static IApplicationBuilder UseMetrics(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MetricsMiddleware>();
    }
}
