using Microsoft.Extensions.Options;
using RESTfulAPI;
using RESTfulAPI.ApiSetup;
using RESTfulAPI.Swagger;
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

services.AddSingleton<CacheConnection>();
services.AddScoped<ICacheService, CacheService>();

services.SetupAuthorisation(config);
services.SetupApplicationContext(config);
services.SetupGateways();
services.SetupCors();

var app = builder.Build();
app.Setup();

app.Run();