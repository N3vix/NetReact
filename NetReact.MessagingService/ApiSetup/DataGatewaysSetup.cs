using NetReact.MessagingService.Gateways;

namespace NetReact.MessagingService.ApiSetup;

internal static class DataGatewaysSetup
{
    public static void SetupGateways(this IServiceCollection services)
    {
        services.AddScoped<IMessagesGateway, MessagesGateway>();
        services.AddScoped<IMessageMediaGetaway, MessageMediaGetaway>();
    }
}