using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Logistics_Supplier1_API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Logistics_Supplier1_API.DTOs.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Logistics_Supplier1_API.Handlers;

/// <summary>
/// Handles authentication-related API requests.
/// </summary>
public static class AuthenticationHandler
{

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <param name="loginRequest">The login request containing username and password.</param>
    /// <returns>A JWT token if authentication is successful.</returns>
    public static async Task<IResult> Authenticate(
        IUserRepository userRepository,
        [FromBody] LoginRequest loginRequest = null)
    {
        if (loginRequest == null || loginRequest.Username == null || loginRequest.Password == null)
        {
            return Results.Json("Please provide your username and password to login.", statusCode: 403);
        }
        Console.WriteLine(Environment.GetEnvironmentVariable("JWT_SECRET_KEY") + "\n\n" + Environment.GetEnvironmentVariable("JWT_SECRET_ISSUER"));
        var user = await userRepository.GetUserByUsernameAsync(loginRequest.Username.ToLower());
        if (user == null)
        {
            return Results.Json("Invalid username or password", statusCode: 401);
        }
        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
        {
            return Results.Json(new { message = "Invalid username or password." }, statusCode: 401);
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),

            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = credentials,
            Issuer = Environment.GetEnvironmentVariable("JWT_SECRET_ISSUER"),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var newJwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(jwtToken);

        return Results.Ok(newJwt.RawData);
    }

}