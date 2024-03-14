﻿using Models;
using MongoDB.Driver;

namespace RESTfulAPI.DB;

public interface IMongoDbContext
{
    IMongoCollection<ServerDetails> Servers { get; }
    IMongoCollection<ServerFollower> Followers { get; }
}