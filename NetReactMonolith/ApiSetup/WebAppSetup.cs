using Microsoft.Extensions.FileProviders;
using NetReact.ServiceSetup;
using NetReactMonolith.Controllers;

namespace NetReactMonolith.ApiSetup;

internal static class WebAppSetup
{
    public static void Setup(this WebApplication app)
    {
        app.SetupCommonApi();
        
        app.MapHub<ChatHub>("/chat");
    }
}