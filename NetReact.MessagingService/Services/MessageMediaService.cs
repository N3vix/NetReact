namespace NetReact.MessagingService.Services;

internal class MessageMediaService : IMessageMediaService
{
    private string ImagesPath { get; }

    public MessageMediaService()
    {
        ImagesPath = AppContext.BaseDirectory + Environment.GetEnvironmentVariable("ASPNETCORE_DBIMAGES");
    }

    public async Task<string> WriteAsync(byte[] image)
    {
        if (image == null || image.Length == 0) return null;

        var imageName = Guid.NewGuid().ToString();
        var newImagePath = Path.Combine(ImagesPath, imageName);
        await using var fileStream = new FileStream(newImagePath, FileMode.Create);
        await fileStream.WriteAsync(image);
        return imageName;
    }
}