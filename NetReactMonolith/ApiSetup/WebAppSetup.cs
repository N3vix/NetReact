using Microsoft.Extensions.FileProviders;
using NetReact.ServiceSetup;
using NetReactMonolith.Controllers;

namespace NetReactMonolith.ApiSetup;

internal static class WebAppSetup
{
    public static void Setup(this WebApplication app)
    {
        var imagesFolderPath = AppContext.BaseDirectory + Environment.GetEnvironmentVariable("ASPNETCORE_DBIMAGES");
        var imagesDirectory = Directory.CreateDirectory(imagesFolderPath);
        // if (string.IsNullOrEmpty(imagesFolder))
        //     Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DbImages");
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(imagesDirectory.FullName),
            RequestPath = "/Attachments",
        });

        app.MapHub<ChatHub>("/chat");
        
        app.SetupCommonApi();
    }
}