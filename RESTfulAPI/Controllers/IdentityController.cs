using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace RESTfulAPI.Controllers;

public class TokenGenerationRequest
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private static TimeSpan TokenLifeTime { get; } = TimeSpan.FromHours(1);

    private IConfiguration Configuration { get; }

    public IdentityController(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validIssuer = Configuration["Authentication:Schemes:Bearer:ValidIssuer"];
        var validAudiences = Configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<string[]>();
        var issuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Configuration["Authentication:Schemes:Bearer:ValidKey"]));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, request.Email),
            new(JwtRegisteredClaimNames.Email, request.Email),
            new("userid", request.UserId.ToString()),
            new("role", request.Role),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            Issuer = validIssuer,
            SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Aud, validAudiences }
            }
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Ok(jwt);
    }
}
