namespace RESTfulAPI.Gateways;

public interface IMessageMediaGetaway
{
    Task<string> WriteAsync(IFormFile formFile);
}