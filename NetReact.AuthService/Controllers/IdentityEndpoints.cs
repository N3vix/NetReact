using Microsoft.AspNetCore.Mvc;
using Models;
using NetReact.AuthService.Services;
using NetReact.ServiceSetup;
using OpenTelemetry.Trace;

namespace NetReact.AuthService.Controllers;

internal static class IdentityEndpoints
{
    public static void SetupIdentityEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("identity");

        group.MapPost("register", Register);
        group.MapPost("login", Login);
        group.MapPost("token", Token);
    }

    private static async Task<IResult> Register(
        [FromBody] UserRegistrationRequest request,
        IdentityService identityService,
        AuthMetrics metrics)
    {
        using var _ = metrics.MeasureRequestDuration();
        var result = await identityService.Register(request.Name, request.Email, request.Password);
        return result.UnpuckResult();
    }

    private static async Task<IResult> Login(
        [FromBody] UserLoginRequest request,
        IdentityService identityService,
        Tracer tracer)
    {
        using var _ = tracer.StartSpan($"{nameof(Login)} endpoint");
        var result = await identityService.Login(request.Email, request.Password);
        return result.UnpuckResult();
    }
    
    private static async Task<IResult> Token(
        [FromBody] TokenGenerationRequest request,
        IdentityService identityService,
        AuthMetrics metrics)
    {
        using var _ = metrics.MeasureRequestDuration();
        var result = await identityService.GenerateToken(request.Name, request.Email);
        return result.UnpuckResult();
    }
}