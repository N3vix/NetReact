using Models;
using NetReact.ChannelManagementService.Repositories;
using OpenTelemetry.Trace;

namespace NetReact.ChannelManagementService.Services;

internal class ChannelsService : IChannelsService
{
    private readonly Tracer _tracer;
    private readonly ILogger<ChannelsService> _logger;
    private readonly ChannelServiceHttpClient _httpClient;
    private readonly IChannelsRepository _channelsRepository;

    public ChannelsService(
        Tracer tracer,
        ILogger<ChannelsService> logger,
        ChannelServiceHttpClient httpClient,
        IChannelsRepository channelsRepository)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(channelsRepository);

        _tracer = tracer;
        _logger = logger;
        _httpClient = httpClient;
        _channelsRepository = channelsRepository;
    }

    public async Task<Result<string, string>> CreateServer(
        string serverId,
        string name,
        ChannelType type)
    {
        using var _ = _tracer.StartSpan(nameof(CreateServer));
        var isFollowing = await IsFollowing(serverId);
        if (isFollowing.IsError)
            return Result<string, string>.Failed(isFollowing.Error);

        var channelId = await _channelsRepository.Add(new ChannelDetails
        {
            Id = Guid.NewGuid().ToString(),
            ServerId = serverId,
            Name = name,
            Type = type
        });

        await _channelsRepository.Save();
        return Result<string, string>.Successful(channelId);
    }

    public async Task<Result<IEnumerable<ChannelDetails>, string>> GetChannels(string serverId)
    {
        using var _ = _tracer.StartSpan(nameof(GetChannels));
        var isFollowing = await IsFollowing(serverId);
        if (isFollowing.IsError)
            return Result<IEnumerable<ChannelDetails>, string>.Failed(isFollowing.Error);

        var channels = await _channelsRepository.GetByServerId(serverId);

        return Result<IEnumerable<ChannelDetails>, string>.Successful(channels);
    }

    public async Task<Result<ChannelDetails, string>> GetChannel(string id)
    {
        using var _ = _tracer.StartSpan(nameof(GetChannel));
        var channel = await _channelsRepository.GetById(id);

        return Result<ChannelDetails, string>.Successful(channel);
    }

    public async Task<Result<bool, string>> GetIsFollowing(string channelId)
    {
        using var _ = _tracer.StartSpan(nameof(GetIsFollowing));
        var channelDetails = await GetChannel(channelId);
        if (channelDetails == null) return "Channel not found.";

        var isFollowing = await IsFollowing(channelDetails.Value.ServerId);
        if (isFollowing.IsError)
            return isFollowing.Error;

        return true;
    }

    private async Task<Result<bool, string>> IsFollowing(string serverId)
    {
        using var _ = _tracer.StartSpan(nameof(IsFollowing));
        var response = await _httpClient.GetIsFollowingServer(serverId);
        if (!response.IsSuccessStatusCode)
            return "Server management service failed.";

        var content = await response.Content.ReadAsStringAsync();
        if (!bool.TryParse(content, out var result) || !result)
            return "Operation not allowed.";

        return true;
    }
}