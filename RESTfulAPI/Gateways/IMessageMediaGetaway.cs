namespace RESTfulAPI.Gateways;

public interface IMessageMediaGetaway
{
    Task<string> WriteMediaAsync(IFormFile formFile);
}