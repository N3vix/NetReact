using NetReact.AuthService.ApiSetup;
using NetReact.AuthService.Controllers;
using NetReact.ServiceSetup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

builder.SetupOpenTelemetry();

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.SetupAuthorisation();
services.SetupAuthentication(config);
services.SetupApplicationContext(config);
services.SetupServices();
services.SetupCors();

var app = builder.Build();
app.SetupIdentityEndpoints();
app.SetupCommonApi();

app.Run();