﻿using Microsoft.Extensions.Options;
using NetReact.ServiceSetup;

namespace NetReact.MessagingService;

public class MessagesServiceHttpClient
{
    private readonly NetReactHttpClient _httpClient;
    private readonly IOptions<Connections> _serviceUrlsOptions;

    public MessagesServiceHttpClient(HttpClient httpClient, IOptions<Connections> serviceUrlsOptions)
    {
        _serviceUrlsOptions = serviceUrlsOptions;

        _httpClient = new NetReactHttpClient(httpClient);
    }

    public async Task<HttpResponseMessage> GetIsFollowingServer(string userId, string channelId)
    {
        return await _httpClient.Get(
            _serviceUrlsOptions.Value.ChannelsService,
            $"channels/{channelId}/user");
    }
}