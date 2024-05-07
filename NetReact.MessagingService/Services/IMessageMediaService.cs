namespace NetReact.MessagingService.Services;

public interface IMessageMediaService
{
    Task<string> WriteAsync(byte[] formFile);
}