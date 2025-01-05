using Logistics_Supplier1_API.DTOs.User;
using Logistics_Supplier1_API.Models;
using Logistics_Supplier1_API.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace Logistics_Supplier1_API.Handlers;

/// <summary>
/// Handles user registration requests.
/// </summary>
public static class RegistrationHandler
{
    /// <summary>
    /// Checks if the create user request is valid.
    /// </summary>
    /// <param name="createUserRequest">The create user request.</param>
    /// <param name="inviteCodeRepository">The repository for accessing invite code data.</param>
    /// <returns>An IResult indicating whether the request is valid.</returns>
    private static IResult IsValidCreateUserRequest(CreateUserRequest createUserRequest, IInviteCodeRepository inviteCodeRepository)
    {
        if (string.IsNullOrEmpty(createUserRequest.Username))
        {
            return Results.BadRequest(new { error = "Username is required" });
        } else if (string.IsNullOrEmpty(createUserRequest.Password))
        {
            return Results.BadRequest(new { error = "Password is required" });
        } else if (string.IsNullOrEmpty(createUserRequest.Address))
        {
            return Results.BadRequest(new { error = "Delivery Address is required" });
        } else if (string.IsNullOrEmpty(createUserRequest.inviteCode))
        {
            return Results.BadRequest(new { error = "Invite code is required" });
        }

        if (createUserRequest.Password.Length < 8 ||
            !createUserRequest.Password.Any(char.IsDigit) ||
            !createUserRequest.Password.Any(char.IsUpper) ||
            !createUserRequest.Password.Any(char.IsLower) ||
            !createUserRequest.Password.Any(char.IsPunctuation))
        {   
            return Results.BadRequest(new { error = "Password must be at least 8 characters long, container one uppercase letter, one lowercase letter, and one special character." });
        }

        var inviteCode = inviteCodeRepository.IsValidInviteCodeAsync(createUserRequest.inviteCode).Result;
        if (inviteCode == null || inviteCode == false)
        {
            return Results.BadRequest(new { error = "Invite code is invalid" });
        }
        
        return Results.Ok();
    }
    
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <param name="inviteCodeRepository">The repository for accessing invite code data.</param>
    /// <param name="createUserRequest">The create user request.</param>
    /// <returns>An IResult indicating the success or failure of the registration.</returns>
    public static async Task<IResult> RegisterUser(
        IUserRepository userRepository,
        IInviteCodeRepository inviteCodeRepository,
        [FromBody] CreateUserRequest createUserRequest = null)
    {
        if (createUserRequest == null || createUserRequest.Username == null || createUserRequest.Password == null || createUserRequest.Address == null || createUserRequest.inviteCode == null)
        {
            return Results.Problem("Please provide a username, password, address and invite code.");
        }
        
        var validResult = IsValidCreateUserRequest(createUserRequest, inviteCodeRepository);
        if (validResult is not Ok) 
        {
            return validResult;
        }
        
        var userCheck = await userRepository.GetUserByUsernameAsync(createUserRequest.Username);
        if (userCheck != null)
        {
            return Results.BadRequest(new { error = "Username is already taken" });
        }
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password);
        var user = new User(createUserRequest.Username.ToLower(), passwordHash, "user", createUserRequest.Address);

        try
        {
            await userRepository.CreateUserAsync(user);
        }
        catch (Exception e)
        {
            return Results.BadRequest(e);
        }
        
        return Results.Json("You have successfully registered. You may now log in.", statusCode: 201);
    }
        
}