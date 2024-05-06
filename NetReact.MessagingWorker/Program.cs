using NetReact.MessageBroker;
using NetReact.MessagingWorker;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.Configure<MessageBrokerConnection>(config.GetSection("MessageBrokerConnection"));

services.AddSingleton<MessageBrokerConnection>();
services.AddSingleton<IMessageBrokerConsumer, MessageConsumer>();
services.AddHostedService<MessagingWorkerService>();

var app = builder.Build();

app.Run();