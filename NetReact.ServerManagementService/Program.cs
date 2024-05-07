using Microsoft.Extensions.Options;
using NetReact.ServerManagementService;
using NetReact.ServerManagementService.ApiSetup;
using NetReact.ServerManagementService.Controllers;
using NetReact.ServiceSetup;
using NetReact.ServiceSetup.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

services.Configure<Connections>(config.GetSection("Connections"));

services.SetupAuthentication(config);
services.SetupCache(config);
services.SetupApplicationContext(config);
services.SetupGateways();
services.SetupCors();

var app = builder.Build();
app.MapServersEndpoints();
app.SetupCommonApi();

app.Run();