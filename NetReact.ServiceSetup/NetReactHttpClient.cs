using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace NetReact.ServiceSetup;

public class NetReactHttpClient
{
    private readonly HttpClient _httpClient;
    
    protected NetReactHttpClient(HttpClient httpClient, string uri)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri(uri);
    }

    public async Task<HttpResponseMessage> Get(string suffixUri, params KeyValuePair<string, string>[] queryParams)
    {
        suffixUri = queryParams.Aggregate($"{suffixUri}?", (s, pair) => $"{s}&{pair.Key}={pair.Value}");

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