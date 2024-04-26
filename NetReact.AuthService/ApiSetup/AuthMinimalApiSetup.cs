using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace NetReact.AuthService.ApiSetup;

internal static class AuthMinimalApiSetup
{
    private static TimeSpan TokenLifeTime { get; } = TimeSpan.FromHours(1);

    public static void SetupAuthApi(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost(
            "/identity/token",
            (TokenGenerationRequest tokenGenerationRequest, IOptionsMonitor<JwtConfig> jwtConfig) 
                => GenerateToken(jwtConfig.CurrentValue, tokenGenerationRequest));
    }

    private static IResult GenerateToken(JwtConfig jwtConfig, TokenGenerationRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.ValidKey));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, request.Email),
                new(JwtRegisteredClaimNames.Email, request.Email),
                new("userid", request.UserId.ToString()),
                new("role", request.Role)
            }),
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            Issuer = jwtConfig.ValidIssuer,
            SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Aud, jwtConfig.ValidAudiences }
            }
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Results.Ok(jwt);
    }
}