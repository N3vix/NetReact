using System.Security.Claims;

namespace Models;

public static class UserIdentityExtensions
{
    public static string GetUserId(this ClaimsPrincipal user) 
        => user.Claims.First(c => c.Type == "userid").Value;
}
