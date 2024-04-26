namespace NetReact.AuthService.ApiSetup;

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

        app.MapControllers();

        app.UseCors("reactApp");
    }
}