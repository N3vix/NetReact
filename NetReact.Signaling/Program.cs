using NetReact.MessageBroker;
using NetReact.ServiceSetup;
using NetReact.Signaling.ApiSetup;
using NetReact.Signaling.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.SetupConfigs(config);
services.AddSingleton<MessageBrokerConnection>();
services.AddScoped<IMessageBrokerConsumerFactory, MessageBrokerConsumerFactory>();
services.AddHostedService<MessagingWorkerService>();

services.AddSignalR();

services.SetupAuthentication(config);
services.SetupApplicationContext(config);
services.SetupCors();

var app = builder.Build();
app.Setup();

app.Run();