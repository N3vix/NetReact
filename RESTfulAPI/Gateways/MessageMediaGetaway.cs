namespace RESTfulAPI.Gateways;

public class MessageMediaGetaway : IMessageMediaGetaway
{
    private string ImagesPath { get; }

    public MessageMediaGetaway()
    {
        ImagesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DbImages");
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
