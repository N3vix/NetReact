using Microsoft.Extensions.FileProviders;
using NetReact.ServiceSetup;
using NetReact.Signaling.Controllers;

namespace NetReact.Signaling.ApiSetup;

internal static class WebAppSetup
{
    public static void Setup(this WebApplication app)
    {
        app.SetupCommonApi();
        
        app.MapHub<ChatHub>("/chat");
    }
}