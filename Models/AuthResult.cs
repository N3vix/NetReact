namespace Models;

public class AuthResult
{
    public bool Success { get; set; }
    public string[] Errors { get; set; }
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
