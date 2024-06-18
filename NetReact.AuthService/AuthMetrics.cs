using System.Diagnostics.Metrics;

namespace NetReact.AuthService;

internal class AuthMetrics
{
    public const string MeterName = "AuthService.Api";

    private readonly Histogram<double> _authApiRequestDuration;

    public AuthMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        _authApiRequestDuration = meter.CreateHistogram<double>($"{MeterName}.requests.duration", "ms");
    }

    public TrackedRequestDuration MeasureRequestDuration()
    {
        return new TrackedRequestDuration(_authApiRequestDuration);
    }
}

internal class TrackedRequestDuration : IDisposable
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