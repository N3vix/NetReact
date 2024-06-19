using System.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;

namespace NetReact.ServiceSetup;

public class Metrics
{
    private readonly Histogram<double> _requestDuration;

    public Metrics(IHostEnvironment environment, IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(environment.ApplicationName);

        _requestDuration = meter.CreateHistogram<double>(
            $"{environment.ApplicationName}.requests.duration", "ms");
    }

    public TrackedRequestDuration MeasureRequestDuration()
    {
        return new TrackedRequestDuration(_requestDuration);
    }
}

public class TrackedRequestDuration : IDisposable
{
    private readonly long _requestStartTime = TimeProvider.System.GetTimestamp();
    
    private readonly Histogram<double> _histogram;

    public TrackedRequestDuration(Histogram<double> histogram)
    {
        _histogram = histogram;
    }

    public void Dispose()
    {
        var elapsed = TimeProvider.System.GetElapsedTime(_requestStartTime);
        _histogram.Record(elapsed.Milliseconds);
    }
}