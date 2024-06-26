using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NetReact.MessageBroker.SharedModels;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;
using RabbitMQ.Client;

namespace NetReact.MessageBroker;

internal class MessageBrokerProducer : MessageBroker, IMessageBrokerProducer
{
    private readonly Tracer _tracer;

    public MessageBrokerProducer(
        Tracer tracer,
        ILogger logger,
        MessageBrokerConnection connection,
        MessageBrokerChannelConnectionConfig channelConnectionConfig)
        : base(logger, connection, channelConnectionConfig)
    {
        _tracer = tracer;
    }

    public void SendMessage<T>(T message)
    {
        using var trace = _tracer.StartSpan($"{ChannelConnectionConfig.Queue} send", SpanKind.Producer);
        
        var props = Channel.CreateBasicProperties();
        
        PropagateTracing(trace, props);

        var body = SerializeMessage(message);
        Channel.BasicPublish(
            ChannelConnectionConfig.Exchange,
            ChannelConnectionConfig.Routing,
            props,
            body);
    }

    private void PropagateTracing(TelemetrySpan telemetrySpan, IBasicProperties properties)
    {
        var contextToInject = telemetrySpan.Context;
        AddMessagingTags(telemetrySpan);

        Propagators.DefaultTextMapPropagator.Inject(
            new PropagationContext(contextToInject, Baggage.Current),
            properties,
            InjectTraceContextIntoBasicProperties);
    }

    private void AddMessagingTags(TelemetrySpan trace)
    {
        trace?.SetAttribute("messaging.system", "rabbitmq");
        trace?.SetAttribute("messaging.destination_kind", "queue");
        trace?.SetAttribute("messaging.destination", ChannelConnectionConfig.Exchange);
        trace?.SetAttribute("messaging.rabbitmq.routing_key", ChannelConnectionConfig.Routing);
    }

    private byte[] SerializeMessage<T>(T message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        return Encoding.UTF8.GetBytes(jsonMessage);
    }

    private void InjectTraceContextIntoBasicProperties(IBasicProperties props, string key, string value)
    {
        try
        {
            props.Headers ??= new Dictionary<string, object>();
            props.Headers[key] = value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to inject trace context.");
        }
    }
}