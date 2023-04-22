using ApiWithDiagnostics.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace ApiWithDiagnostics;

public class LogRequestTimeFilterAttribute : ActionFilterAttribute
{
    readonly Stopwatch _stopwatch = new();

    public override void OnActionExecuting(ActionExecutingContext context) => _stopwatch.Start();

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch.Stop();

        EventCounterSource.Log.RequestTime(
            context.HttpContext.Request.GetDisplayUrl(),
            _stopwatch.ElapsedMilliseconds
        );
    }
}
