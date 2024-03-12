﻿using Models;

namespace RESTfulAPI.Gateways;

public interface IServersGateway
{
    Task Add(params ServerDetails[] serverDetails);
    Task Edit(string id, Action<ServerDetails> editor);
    Task<ServerDetails[]> GetAll();
    Task<ServerDetails[]> GetByServerId(string[] ids);
    Task<ServerDetails[]> GetByUserId(string userId);
    Task<ServerDetails> GetByServerId(string id);
}