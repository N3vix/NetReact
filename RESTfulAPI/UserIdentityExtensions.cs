using System.Security.Claims;

namespace RESTfulAPI;

internal static class UserIdentityExtensions
{
    public static string GetUserId(this ClaimsPrincipal user) 
        => user.Claims.First(c => c.Type == "userid").Value;
}
