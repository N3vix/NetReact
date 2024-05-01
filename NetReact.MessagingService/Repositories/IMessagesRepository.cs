﻿using Models;

namespace NetReact.MessagingService.Repositories;

public interface IMessagesRepository
{
    Task<string> Add(ChannelMessage channelDetails);
    Task<IEnumerable<ChannelMessage>> Get(string channelId, int take, DateTime? from);
    Task<ChannelMessage> GetById(string id);
    Task<bool> Edit(string id, ChannelMessage message);
    Task<bool> Delete(string id);
}