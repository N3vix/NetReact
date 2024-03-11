using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using RESTfulAPI.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RESTfulAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthManagementController : ControllerBase
{
    private readonly ILogger<AuthManagementController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfig _jwtConfig;

    public AuthManagementController(
        ILogger<AuthManagementController> logger,
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
            return BadRequest(new RegistrationRequestResponse
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

        return Ok(new RegistrationRequestResponse
        {
            Success = true,
            Token = token
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

        return Ok(new LoginRequestResponse
        {
            Success = true,
            Token = token
        });
    }

    private string GenerateToken(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.ValidKey));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("userid", user.Id),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _jwtConfig.ValidIssuer,
            SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Aud, _jwtConfig.ValidAudiences }
            }
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return jwt;
    }
}
