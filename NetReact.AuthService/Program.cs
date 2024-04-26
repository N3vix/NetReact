using NetReact.AuthService.ApiSetup;
using NetReact.ServiceSetup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.SetupAuthorisation();
services.SetupAuthentication(config);
services.SetupApplicationContext(config);
services.SetupCors();

var app = builder.Build();
app.Setup();

app.Run();