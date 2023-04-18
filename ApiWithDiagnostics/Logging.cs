using Microsoft.Diagnostics.EventFlow;
using Microsoft.Diagnostics.EventFlow.Inputs;

namespace ApiWithDiagnostics;

public static class Logging
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        var pipeline = DiagnosticPipelineFactory.CreatePipeline(@".\eventFlowConfig.json");
        builder.Logging.AddEventFlow(pipeline);
    }
}
