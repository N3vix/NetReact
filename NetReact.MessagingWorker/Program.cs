using NetReact.MessageBroker;
using NetReact.MessagingWorker.ApiSetup;
using NetReact.MessagingWorker.Services;
using NetReact.ServiceSetup;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

builder.SetupOpenTelemetry();

services.SetupConfigs(config);
services.AddSingleton<MessageBrokerConnection>();
services.AddScoped<IMessageBrokerConsumerFactory, MessageBrokerConsumerFactory>();
services.AddScoped<IMessageBrokerProducerFactory, MessageBrokerProducerFactory>();

services.AddScoped<IMessageConsumerHandler, CreateChannelMessageConsumerHandler>();
services.AddScoped<IMessageConsumerHandler, EditChannelMessageConsumerHandler>();
services.AddScoped<IMessageConsumerHandler, DeleteMessageConsumerHandler>();

services.AddHostedService<MessagingWorkerService>();

services.SetupApplicationContext(config);
services.SetupServices();

var app = builder.Build();

app.Run();