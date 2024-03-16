using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RESTfulAPI.Configurations;
using RESTfulAPI.Controllers;
using RESTfulAPI.DB;
using RESTfulAPI.Gateways;
using RESTfulAPI.Repositories;
using RESTfulAPI.Repositories.MongoDB;
using RESTfulAPI.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Add coservices to the container.

services.Configure<JwtConfig>(builder.Configuration.GetSection("Authentication:Schemes:Bearer"));

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
services.AddSignalR();

services
    .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationContext>();
services
    .AddAuthentication(ConfigureAuthentication)
    .AddJwtBearer(ConfigureJwtBearer);
services.AddAuthorization();

services.AddSingleton<IMongoDbContext, MongoDbContext>(x => new MongoDbContext(config.GetConnectionString("MongoDB")));

services.AddDbContext<ApplicationContext>(ConfigureApplicationContextOptions);

services.AddSingleton<IMessagesRepository, MessagesRepository>();

services.AddScoped<IServersRepository, ServersRepositoryMongoDB>();
services.AddScoped<IChannelsRepository, ChannelsRepositoryMongoDB>();
services.AddScoped<IChannelMessagesRepository, ChannelMessagesRepositoryMongoDb>();

services.AddScoped<IChannelMessagesGateway, ChannelMessagesGateway>();

services.AddCors(opt =>
{
    opt.AddPolicy("reactApp", builder =>
    {
        builder
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseCors("reactApp");

app.MapHub<ChatHub>("/chat");

app.Run();

void ConfigureAuthentication(Microsoft.AspNetCore.Authentication.AuthenticationOptions options)
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}

void ConfigureJwtBearer(JwtBearerOptions x)
{
    x.SaveToken = true;
    x.TokenValidationParameters = GetTokenValidationParameters();
    x.Events = GetJwtBearerEvents();
}

TokenValidationParameters GetTokenValidationParameters()
{
    return new TokenValidationParameters
    {
        ValidIssuer = config["Authentication:Schemes:Bearer:ValidIssuer"],
        ValidAudiences = config.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<string[]>(),
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Authentication:Schemes:Bearer:ValidKey"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
    };
}

JwtBearerEvents GetJwtBearerEvents() => new() { OnMessageReceived = PopulateAccessToken };

Task PopulateAccessToken(MessageReceivedContext messageContext)
{
    var accessToken = messageContext.Request.Query["access_token"];
    var path = messageContext.HttpContext.Request.Path;
    if (!string.IsNullOrEmpty(accessToken)
        && path.StartsWithSegments("/chat"))
    {
        messageContext.Token = accessToken;
    }
    return Task.CompletedTask;
}

void ConfigureApplicationContextOptions(DbContextOptionsBuilder options)
    => options.UseSqlite(builder.Configuration.GetConnectionString("Database"));