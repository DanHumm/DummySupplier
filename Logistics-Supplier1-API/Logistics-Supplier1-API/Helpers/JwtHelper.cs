using System.Security.Claims;

namespace Logistics_Supplier1_API.Helpers;

/// <summary>
/// Provides helper methods for working with JWT (JSON Web Token).
/// </summary>
public class JwtHelper
{
    /// <summary>
    /// Gets the user ID from the JWT token in the HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The user ID if found in the JWT token, otherwise null.</returns>
    public static int? GetUserIdFromJwt(HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return null;
        }

        return userId;
    }

    /// <summary>
    /// Gets the user's role from the JWT token in the HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The user's role if found in the JWT token, otherwise null.</returns>
    public static string GetRoleFromJwt(HttpContext httpContext)
    {
        var userRoleClaim = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
        if (userRoleClaim == null)
        {
            return null;
        }

        return userRoleClaim.Value;
    }
}