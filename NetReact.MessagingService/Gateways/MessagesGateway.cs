﻿using Models;
using NetReact.MessagingService.Repositories;

namespace NetReact.MessagingService.Gateways;

public class MessagesGateway : IMessagesGateway
{
    private ILogger<MessagesGateway> Logger { get; }
    private IMessagesRepository MessagesRepository { get; }

    public MessagesGateway(
        ILogger<MessagesGateway> logger,
        IMessagesRepository messagesRepository)
    {
        Logger = logger;
        MessagesRepository = messagesRepository;
    }

    public async Task<string> Add(string senderId, string channelId, string content, string image)
    {
        var channelMessage = new ChannelMessage
        {
            ChannelId = channelId,
            SenderId = senderId,
            Timestamp = DateTime.UtcNow,
            Content = content,
            Image = image
        };

        return await MessagesRepository.Add(channelMessage);
    }

    public async Task<ChannelMessage> Get(string messageId)
    {
        return await MessagesRepository.GetById(messageId);
    }

    public Task<IEnumerable<ChannelMessage>> Get(string channelId, int take, DateTime from)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ChannelMessage>> Get(string channelId, int take, DateTime? from = null)
    {
        return await MessagesRepository.Get(channelId, take, from);
    }

    public async Task<bool> Update(string senderId, string messageId, string newContent)
    {
        if (string.IsNullOrEmpty(newContent)) return false;

        var message = await Get(messageId);
        if (!senderId.Equals(message.SenderId)) return false;

        message.Content = newContent;
        message.EditedTimestamp = DateTime.UtcNow;
        return await MessagesRepository.Edit(messageId, message);
    }

    public async Task<bool> Delete(string senderId, string messageId)
    {
        var message = await Get(messageId);

        if (!senderId.Equals(message.SenderId)) return false;
        return await MessagesRepository.Delete(messageId);
    }
}