using NetReact.ChannelManagementService;
using NetReact.ChannelManagementService.ApiSetup;
using NetReact.ChannelManagementService.Controllers;
using NetReact.ServiceSetup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.Configure<Connections>(config.GetSection("Connections"));

services.AddHttpClient<ChannelServiceHttpClient>().AddHeaderPropagation();
services.AddHeaderPropagation(o => o.Headers.Add("Authorization"));

services.SetupAuthentication(config);
services.SetupApplicationContext(config);
services.SetupGateways();
services.SetupCors();

var app = builder.Build();
app.SetupChannelsEndpoints();
app.SetupCommonApi();
app.UseHeaderPropagation();

app.Run();