using NetReact.MessageBroker;
using NetReact.MessageBroker.SharedModels;
using NetReact.MessagingWorker;
using NetReact.MessagingWorker.ApiSetup;
using NetReact.MessagingWorker.Services;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.Configure<Connections>(config.GetSection("Connections"));
services.Configure<MessageBrokerConnectionConfig>(config.GetSection("MessageBrokerConnection"));
services.Configure<MessageBrokerChannelConnectionConfig>(
    "MessageCreated",
    config.GetSection("MessageBrokerChannelConnections:MessageCreated"));

services.AddSingleton<MessageBrokerConnection>();
services.AddScoped<IMessageBrokerConsumerFactory, MessageBrokerConsumerFactory>();
services.AddHostedService<MessagingWorkerService>();

services.SetupApplicationContext(config);
services.SetupGateways();

var app = builder.Build();

app.Run();