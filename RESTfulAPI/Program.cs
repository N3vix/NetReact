using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RESTfulAPI;
using RESTfulAPI.Configurations;
using RESTfulAPI.Controllers;
using RESTfulAPI.Gateways;
using RESTfulAPI.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add coservices to the container.

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Authentication:Schemes:Bearer"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSignalR();

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationContext>();
builder.Services
    .AddAuthentication(ConfigureAuthentication)
    .AddJwtBearer(ConfigureJwtBearer);
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationContext>(ConfigureApplicationContextOptions);
builder.Services.AddScoped<IServersGateway, ServersGateway>();
builder.Services.AddSingleton<IMessagesGateway, MessagesGateway>();

builder.Services.AddCors(opt =>
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