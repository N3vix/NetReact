using System.Text;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NetReact.MessageBroker;

internal class MessageBrokerConsumer : MessageBroker, IMessageBrokerConsumer
{
    private readonly Tracer _tracer;
    private readonly IMessageConsumerHandler _consumerHandler;
    private readonly EventingBasicConsumer _consumer;

    public MessageBrokerConsumer(
        Tracer tracer,
        ILogger logger,
        MessageBrokerConnection connection,
        IMessageConsumerHandler consumerHandler)
        : base(logger, connection, consumerHandler.Config)
    {
        _tracer = tracer;
        _consumerHandler = consumerHandler;
        _consumer = new EventingBasicConsumer(Channel);

        InitMessageReceiving();
    }

    private void InitMessageReceiving()
    {
        _consumer.Received += OnMessageReceived;
        Channel.BasicConsume(
            ChannelConnectionConfig.Queue,
            true, Guid.NewGuid().ToString(), false, false, null,
            _consumer);
    }

    private async void OnMessageReceived(object? @object, BasicDeliverEventArgs args)
    {
        var propagationContext = GetPropagationContext(args);
        using var _ = _tracer.StartActiveSpan($"{ChannelConnectionConfig.Queue} receive", SpanKind.Consumer,
            new SpanContext(propagationContext.ActivityContext));

        var message = GetMessage(args);
        await _consumerHandler.Handle(message);
    }

    private PropagationContext GetPropagationContext(BasicDeliverEventArgs args)
    {
        var parentContext = Propagators.DefaultTextMapPropagator.Extract(
            default,
            args.BasicProperties,
            ExtractTraceContextFromBasicProperties);
        Baggage.Current = parentContext.Baggage;
        return parentContext;
    }

    private string GetMessage(BasicDeliverEventArgs args)
    {
        var body = args.Body.ToArray();
        return Encoding.UTF8.GetString(body);
    }

    private IEnumerable<string> ExtractTraceContextFromBasicProperties(IBasicProperties props, string key)
    {
        try
        {
            if (props.Headers.TryGetValue(key, out var value))
            {
                var bytes = value as byte[];
                return new[] { Encoding.UTF8.GetString(bytes) };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract trace context.");
        }

        return Enumerable.Empty<string>();
    }

    public override void Dispose()
    {
        _consumer.Received -= OnMessageReceived;
        base.Dispose();
    }
}