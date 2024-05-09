using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace NetReact.ServiceSetup;

public static class WebAppExtensions
{
    public static void SetupCommonApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("reactApp");
    }
}