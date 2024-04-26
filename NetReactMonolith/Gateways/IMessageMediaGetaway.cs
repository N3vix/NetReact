namespace NetReactMonolith.Gateways;

public interface IMessageMediaGetaway
{
    Task<string> WriteAsync(IFormFile formFile);
}