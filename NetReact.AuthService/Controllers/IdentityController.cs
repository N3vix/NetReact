using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace NetReact.AuthService.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly ILogger<IdentityController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfig _jwtConfig;

    public IdentityController(
        ILogger<IdentityController> logger,
        UserManager<IdentityUser> userManager,
        IOptionsMonitor<JwtConfig> optionsMonitor)
    {
        _logger = logger;
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid request payload");
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return BadRequest(new UserRegistrationResponse
            {
                Success = false,
                Errors = ["User already exist"]
            });

        var newUser = new IdentityUser
        {
            Email = request.Email,
            UserName = request.Name
        };

        var isCreated = await _userManager.CreateAsync(newUser, request.Password);
        if (!isCreated.Succeeded)
            return BadRequest("Failed to create the user, please try again later");

        var token = GenerateToken(newUser);

        return Ok(new UserRegistrationResponse
        {
            Success = true,
            Token = token,
            UserId = newUser.Id
        });
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid request payload");

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser == null)
            return BadRequest("User not exist");

        var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, request.Password);
        if (!isPasswordValid)
            return BadRequest("Invalid user password");

        var token = GenerateToken(existingUser);

        return Ok(new UserLoginResponse
        {
            Success = true,
            Token = token,
            UserId = existingUser.Id
        });
    }

    private string GenerateToken(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = GetTokenDescriptor(user);

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return jwt;
    }

    private SecurityTokenDescriptor GetTokenDescriptor(IdentityUser user)
    {
        var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.ValidKey));
        return new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _jwtConfig.ValidIssuer,
            SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256),
            Claims = GetClaims(user)
        };
    }

    private IDictionary<string, object> GetClaims(IdentityUser user)
    {
        return new Dictionary<string, object>
        {
            { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()},
            { JwtRegisteredClaimNames.Sub, user.Email },
            { JwtRegisteredClaimNames.Email, user.Email },
            { "userid", user.Id },
            { JwtRegisteredClaimNames.Aud, _jwtConfig.ValidAudiences }
        };
    }
}
