using System.Diagnostics.Tracing;

namespace ApiWithDiagnostics.Diagnostics;

[EventSource(Name = "ApiWithDiagnostics.Metrics")]
public class EventCounterSource : EventSource
{
    public static readonly EventCounterSource Log = new();

    private readonly EventCounter _requestTimeCounter;

    private EventCounterSource()
    {
        _requestTimeCounter = new EventCounter("RequestTime", this)
        {
            DisplayName = "Request processing time",
            DisplayUnits = "ms"
        };
    }

    public void RequestTime(string url, long milliseconds)
    {
        WriteEvent(1, url, milliseconds);
        _requestTimeCounter?.WriteMetric(milliseconds);
    }

    protected override void Dispose(bool disposing)
    {
        _requestTimeCounter?.Dispose();
        base.Dispose(disposing);
    }
}
