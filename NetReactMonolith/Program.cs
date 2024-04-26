using Microsoft.Extensions.Options;
using NetReact.ServiceSetup;
using NetReactMonolith.ApiSetup;
using NetReactMonolith.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
services.AddSignalR();

services.SetupAuthentication(config);
services.SetupCache(config);
services.SetupApplicationContext(config);
services.SetupGateways();
services.SetupCors();

var app = builder.Build();
app.Setup();

app.Run();