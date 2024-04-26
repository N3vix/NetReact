namespace NetReact.AuthService.Configurations;

public class JwtConfig
{
    public string ValidKey { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string[] ValidAudiences { get; set; } = null;
}