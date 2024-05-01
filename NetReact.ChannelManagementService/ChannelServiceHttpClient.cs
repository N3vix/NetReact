using Microsoft.Extensions.Options;
using NetReact.ServiceSetup;

namespace NetReact.ChannelManagementService;

public class ChannelServiceHttpClient
{
    private readonly NetReactHttpClient _httpClient;
    private readonly IOptions<Connections> _serviceUrlsOptions;

    public ChannelServiceHttpClient(HttpClient httpClient, IOptions<Connections> serviceUrlsOptions)
    {
        _serviceUrlsOptions = serviceUrlsOptions;

        _httpClient = new NetReactHttpClient(httpClient);
    }

    public async Task<HttpResponseMessage> GetIsFollowingServer(string serverId)
    {
        return await _httpClient.Get(
            _serviceUrlsOptions.Value.ServersService,
            $"servers/{serverId}/user");
    }
}