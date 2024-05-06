using NetReact.MessagingWorker.Gateways;

namespace NetReact.MessagingWorker.ApiSetup;

internal static class DataGatewaysSetup
{
    public static void SetupGateways(this IServiceCollection services)
    {
        services.AddScoped<IMessagesGateway, MessagesGateway>();
    }
}