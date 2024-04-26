using System.Security.Claims;

namespace NetReactMonolith;

internal static class UserIdentityExtensions
{
    public static string GetUserId(this ClaimsPrincipal user) 
        => user.Claims.First(c => c.Type == "userid").Value;
}
