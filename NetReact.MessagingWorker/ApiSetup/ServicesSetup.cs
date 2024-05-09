using NetReact.MessagingWorker.Services;

namespace NetReact.MessagingWorker.ApiSetup;

internal static class ServicesSetup
{
    public static void SetupServices(this IServiceCollection services)
    {
        services.AddScoped<IMessagesService, MessagesService>();
    }
}