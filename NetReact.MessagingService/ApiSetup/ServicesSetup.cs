using NetReact.MessagingService.Services;

namespace NetReact.MessagingService.ApiSetup;

internal static class ServicesSetup
{
    public static void SetupServices(this IServiceCollection services)
    {
        services.AddScoped<IMessagesService, MessagesService>();
        services.AddScoped<IMessageMediaService, MessageMediaService>();
    }
}