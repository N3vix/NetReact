using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using OpenTelemetry.Trace;

namespace NetReact.AuthService.Services;

internal class IdentityService
{
    private readonly Tracer _tracer;
    private readonly ILogger<IdentityService> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfig _jwtConfig;

    public IdentityService(
        Tracer tracer,
        ILogger<IdentityService> logger,
        UserManager<IdentityUser> userManager,
        IOptionsMonitor<JwtConfig> optionsMonitor)
    {
        _tracer = tracer;
        _logger = logger;
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;
    }

    public async Task<Result<UserRegistrationResponse, string>> Register(string name, string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
            return "User already exist";

        var newUser = new IdentityUser
        {
            Email = email,
            UserName = name
        };

        var isCreated = await _userManager.CreateAsync(newUser, password);
        if (!isCreated.Succeeded)
            return "Failed to create the user, please try again later";

        var token = GenerateToken(newUser);

        return new UserRegistrationResponse
        {
            Success = true,
            Token = token,
            UserId = newUser.Id
        };
    }

    public async Task<Result<UserLoginResponse, string>> Login(string email, string password)
    {
        var existingUser = await FindUserByEmail(email);
        if (existingUser == null)
            return "User not exist";

        var isPasswordValid = await CheckUserPassword(existingUser, password);
        if (!isPasswordValid)
            return "Invalid user password";

        var token = GenerateToken(existingUser);

        return new UserLoginResponse
        {
            Success = true,
            Token = token,
            UserId = existingUser.Id
        };
    }

    private async Task<IdentityUser?> FindUserByEmail(string email)
    {
        using var _ = _tracer.StartSpan(nameof(FindUserByEmail));
        return await _userManager.FindByEmailAsync(email);
    }

    private async Task<bool> CheckUserPassword(IdentityUser user, string password)
    {
        using var _ = _tracer.StartSpan(nameof(CheckUserPassword));
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<Result<string, string>> GenerateToken(string name, string email)
    {
        var newUser = new IdentityUser
        {
            Email = email,
            UserName = name
        };

        return Result<string,string>.Successful(GenerateToken(newUser));
    }

    private string GenerateToken(IdentityUser user)
    {
        using var _ = _tracer.StartSpan(nameof(GenerateToken));
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
            { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() },
            { JwtRegisteredClaimNames.Sub, user.Email },
            { JwtRegisteredClaimNames.Email, user.Email },
            { "userid", user.Id },
            { JwtRegisteredClaimNames.Aud, _jwtConfig.ValidAudiences }
        };
    }
}