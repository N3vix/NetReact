using NetReact.MessagingWorker.Services;

namespace NetReact.MessagingWorker.ApiSetup;

internal static class DataGatewaysSetup
{
    public static void SetupGateways(this IServiceCollection services)
    {
        services.AddScoped<IMessagesService, MessagesService>();
    }
}