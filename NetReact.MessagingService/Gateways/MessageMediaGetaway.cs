namespace NetReact.MessagingService.Gateways;

internal class MessageMediaGetaway : IMessageMediaGetaway
{
    private string ImagesPath { get; }

    public MessageMediaGetaway()
    {
        ImagesPath = AppContext.BaseDirectory + Environment.GetEnvironmentVariable("ASPNETCORE_DBIMAGES");
    }

    public async Task<string> WriteAsync(IFormFile formFile)
    {
        if (formFile == null) return null;

        var newImagePath = Path.Combine(ImagesPath, formFile.FileName);
        using var fileStream = new FileStream(newImagePath, FileMode.Create);
        await formFile.CopyToAsync(fileStream);
        return formFile.FileName;
    }
}
