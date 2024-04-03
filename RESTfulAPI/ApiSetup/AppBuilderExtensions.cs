using Microsoft.Extensions.FileProviders;
using RESTfulAPI.Controllers;

namespace RESTfulAPI.ApiSetup;

internal static class ApplicationExtensions
{
    public static void Setup(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "DbImages")),
            RequestPath = "/Attachments",
        });

        app.MapControllers();

        //app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        app.UseCors("reactApp");

        app.MapHub<ChatHub>("/chat");
    }
}
