using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace NetReact.AuthService.ApiSetup;

internal static class OpenTelemetryBuilder
{
    public static void SetupOpenTelemetry(this WebApplicationBuilder builder)
    {
        builder.Logging.SetupOpenTelemetry();
        
        var services = builder.Services;
        services.Configure<OpenTelemetryLoggerOptions>(
            x => x.AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317")));
        services.AddOpenTelemetry()
            .WithMetrics(x =>
            {
                x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317"));

                x.AddMeter("AuthService.Api");
        
                x.AddView("request-duration",
                    new ExplicitBucketHistogramConfiguration
                    {
                        Boundaries = [0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.7, 1]
                    });
            })
            .WithTracing(x =>
            {
                x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317"));

                x.AddSource("AuthService.Api");
            });
        services.AddSingleton<AuthMetrics>();
        services.AddSingleton(TracerProvider.Default.GetTracer(AuthMetrics.MeterName));
    }

    private static void SetupOpenTelemetry(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.AddOpenTelemetry(x =>
        {
            x.IncludeScopes = true;
            x.IncludeFormattedMessage = true;
        });
    }
}