using NetReact.ChannelManagementService.ApiSetup;
using NetReact.ServiceSetup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.SetupAuthentication(config);
services.SetupApplicationContext(config);
services.SetupGateways();
services.SetupCors();

var app = builder.Build();
app.SetupCommonApi();

app.Run();