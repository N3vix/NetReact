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

services.Configure<ServiceUrls>(config.GetSection("ServiceUrls"));

services.AddHttpClient<MessagesServiceHttpClient>().AddHeaderPropagation();
services.AddHeaderPropagation(o => o.Headers.Add("Authorization"));

services.SetupAuthentication(config);
services.SetupApplicationContext(config);
services.SetupCors();

var app = builder.Build();
app.SetupCommonApi();
app.UseHeaderPropagation();

app.Run();