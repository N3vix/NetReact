namespace NetReact.MessagingService.Gateways;

public interface IMessageMediaGetaway
{
    Task<string> WriteAsync(IFormFile formFile);
}