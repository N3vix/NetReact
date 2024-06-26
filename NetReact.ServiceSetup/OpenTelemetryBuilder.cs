using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace NetReact.ServiceSetup;

public static class OpenTelemetryBuilder
{
    public static void SetupOpenTelemetry(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var logging = builder.Logging;

        logging.AddOpenTelemetry(ConfigureLoggingTelemetry);
        services.AddOpenTelemetry()
            .WithMetrics(x => ConfigureMetrics(x, builder))
            .WithTracing(x => ConfigureTracing(x, builder));

        services.AddSingleton<Metrics>();
        services.AddSingleton(TracerProvider.Default.GetTracer(builder.Environment.ApplicationName));

        builder.ConfigureExporters();
    }

    private static void ConfigureLoggingTelemetry(OpenTelemetryLoggerOptions options)
    {
        options.IncludeScopes = true;
        options.IncludeFormattedMessage = true;
    }

    private static void ConfigureMetrics(MeterProviderBuilder meterProviderBuilder, IHostApplicationBuilder builder)
    {
        meterProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
            .AddRuntimeInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddMeter(builder.Environment.ApplicationName);

        meterProviderBuilder.AddView("request-duration",
            new ExplicitBucketHistogramConfiguration
            {
                Boundaries = [0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.7, 1]
            });
    }

    private static void ConfigureTracing(TracerProviderBuilder tracerProviderBuilder, IHostApplicationBuilder builder)
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddSource(builder.Environment.ApplicationName);
    }

    private static void ConfigureExporters(this IHostApplicationBuilder builder)
    {
        var endpoint = builder.Configuration["Connections:OTEL_EXPORTER_OTLP_ENDPOINT"];

        if (string.IsNullOrWhiteSpace(endpoint)) return;
        var uri = new Uri(endpoint);

        builder.Services.Configure<OpenTelemetryLoggerOptions>(x => x.AddOtlpExporter(opts => opts.Endpoint = uri));
        builder.Services.ConfigureOpenTelemetryMeterProvider(x => x.AddOtlpExporter(opts => opts.Endpoint = uri));
        builder.Services.ConfigureOpenTelemetryTracerProvider(x => x.AddOtlpExporter(opts => opts.Endpoint = uri));
    }
}