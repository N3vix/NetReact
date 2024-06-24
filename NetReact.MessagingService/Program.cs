using NetReact.MessagingService;
using NetReact.MessagingService.ApiSetup;
using NetReact.MessagingService.Controllers;
using NetReact.ServiceSetup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

builder.SetupOpenTelemetry();

services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.SetupConfigs(config);

services.AddHttpClient<MessagesServiceHttpClient>().AddHeaderPropagation();
services.AddHeaderPropagation(o => o.Headers.Add("Authorization"));

services.SetupAuthentication(config);
services.SetupApplicationContext(config);
services.SetupServices();
services.SetupMessageBroker();
services.SetupCors();

var app = builder.Build();
app.Setup();
app.UseHeaderPropagation();
app.MapChannelMessagesEndpoints();

app.Run();