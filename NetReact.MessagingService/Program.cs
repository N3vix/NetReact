using NetReact.MessageBroker;
using NetReact.MessagingService;
using NetReact.MessagingService.ApiSetup;
using NetReact.ServiceSetup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.Configure<Connections>(config.GetSection("Connections"));
services.Configure<MessageBrokerConnection>(config.GetSection("MessageBrokerConnection"));

services.AddHttpClient<MessagesServiceHttpClient>().AddHeaderPropagation();
services.AddHeaderPropagation(o => o.Headers.Add("Authorization"));

services.SetupAuthentication(config);
services.SetupApplicationContext(config);
services.SetupGateways();
services.SetupMessageBroker();
services.SetupCors();

var app = builder.Build();
app.Setup();
app.UseHeaderPropagation();

app.Run();