using Microsoft.Extensions.FileProviders;
using NetReactMonolith.Controllers;

namespace NetReactMonolith.ApiSetup;

internal static class WebAppConfigurator
{
    public static void Setup(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        var imagesFolderPath = AppContext.BaseDirectory + Environment.GetEnvironmentVariable("ASPNETCORE_DBIMAGES");
        var imagesDirectory = Directory.CreateDirectory(imagesFolderPath);
        // if (string.IsNullOrEmpty(imagesFolder))
        //     Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DbImages");
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(imagesDirectory.FullName),
            RequestPath = "/Attachments",
        });

        app.MapControllers();

        //app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        app.UseCors("reactApp");

        app.MapHub<ChatHub>("/chat");
    }
}