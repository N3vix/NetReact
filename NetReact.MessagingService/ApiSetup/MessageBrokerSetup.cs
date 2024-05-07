using NetReact.MessageBroker;

namespace NetReact.MessagingService.ApiSetup;

internal static class MessageBrokerSetup
{
    public static void SetupMessageBroker(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MessageBrokerConnection>();
        serviceCollection.AddScoped<IMessageBrokerProducerFactory, MessageBrokerProducerFactory>();
    }
}