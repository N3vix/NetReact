using NetReact.MessageBroker;
using NetReact.MessagingWorker.ApiSetup;
using NetReact.MessagingWorker.Services;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.SetupConfigs(config);
services.AddSingleton<MessageBrokerConnection>();
services.AddScoped<IMessageBrokerConsumerFactory, MessageBrokerConsumerFactory>();
services.AddScoped<IMessageBrokerProducerFactory, MessageBrokerProducerFactory>();
services.AddHostedService<MessagingWorkerService>();

services.SetupApplicationContext(config);
services.SetupServices();

var app = builder.Build();

app.Run();