using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace NetReact.ServiceSetup;

public class NetReactHttpClient
{
    private readonly HttpClient _httpClient;

    public NetReactHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> Get(
        string uri,
        [StringSyntax("Uri")] string suffixUri,
        params (string key, string value)[] queryParams)
    {
        suffixUri = queryParams.Aggregate($"{uri}/{suffixUri}?", (s, pair) => $"{s}&{pair.key}={pair.value}");

        return await _httpClient.GetAsync(suffixUri);
    }

    public async Task<HttpResponseMessage> Post<T>(string suffixUri, T content)
    {
        var requestContent = new StringContent(
            JsonSerializer.Serialize(content),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        return await _httpClient.PostAsync(suffixUri, requestContent);
    }
}