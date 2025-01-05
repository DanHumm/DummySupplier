using Logistics_Supplier1_API.DTOs.User;
using Logistics_Supplier1_API.Models;
using Logistics_Supplier1_API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logistics_Supplier1_API.Handlers;

/// <summary>
/// Handles API requests related to users.
/// </summary>
public static class UserHandlers
{
    /// <summary>
    /// Gets a user by their username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <returns>The user with the specified username.</returns>
   [Authorize(Roles = "admin")]
   public static async Task<IResult> GetUserByUsernameAsync(string username, IUserRepository userRepository)
   {
      User user = await userRepository.GetUserByUsernameAsync(username);
      if (user == null)
      {
         return Results.NotFound("User not found");
      }
      // Creating new object to remove PasswordHash property for response
      var userDto = new
      {
         Id = user.Id,
         Username = user.Username,
         Role = user.Role,
         Address = user.Address,

      };
      return Results.Ok(userDto);
   }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <returns>A list of all users.</returns>
   [Authorize(Roles = "admin")]
   public static async Task<IResult> GetAllUsersAsync(IUserRepository userRepository)
   {
      var users = await userRepository.GetAllUsersAsync();
      if (users == null)
      {
         return Results.NotFound("No users found");
      }

      return Results.Ok(users);
   }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="username">The username of the user to update.</param>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <param name="req">The update user request.</param>
    /// <returns>The updated user.</returns>
   [Authorize(Roles = "admin")]
   public static async Task<IResult> UpdateUserAsync(string username,
      IUserRepository userRepository,
      [FromBody] UpdateUserRequest req = null)
   {
      if (req == null)
      {
         return Results.BadRequest("A request body is required");
      }
      if (string.IsNullOrEmpty(username))
      {
         return Results.BadRequest("Username is required");
      }
      var user = await userRepository.GetUserByUsernameAsync(username);
      if (user == null)
      {
         return Results.NotFound("User not found");
      }
      Console.WriteLine(req.Role + " - " + req.Address);
      if (string.IsNullOrEmpty(req.Role) && string.IsNullOrEmpty(req.Address))
      {
         return Results.BadRequest("At least one change is required (Role or Address)");
      }
      else if (string.IsNullOrEmpty(req.Role) && !string.IsNullOrEmpty(req.Address))
      {
         user.UpdateUser(null, req.Address);
      } 
      else if (!string.IsNullOrEmpty(req.Role) && string.IsNullOrEmpty(req.Address))
      {
         user.UpdateUser(req.Role, null);
      } 
      else if (!string.IsNullOrEmpty(req.Role) && !string.IsNullOrEmpty(req.Address))
      {
         user.UpdateUser(req.Role, req.Address);
      }
      else
      {
         return Results.BadRequest("Something went wrong. Please check your request and try again");
      }
      
      try
      {
         await userRepository.UpdateUserAsync(user);
      }
      catch (Exception e)
      {
        return Results.Problem("An error occured while updating the user", statusCode: 500);
      }
      
      var userDto = new
      {
         Id = user.Id,
         Username = user.Username,
         Role = user.Role,
         Address = user.Address,

      };
      return Results.Ok(userDto);
   }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="username">The username of the user to delete.</param>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <returns>A result indicating whether the user was deleted successfully.</returns>
   [Authorize(Roles = "admin")]
   public static async Task<IResult> DeleteUserAsync(string username, IUserRepository userRepository)
   {
      var user = await userRepository.GetUserByUsernameAsync(username);
      if (user == null)
      {
         return Results.NotFound("User not found");
      }

      try
      {
         await userRepository.DeleteUserAsync(user);
      }
      catch (Exception e)
      {
         return Results.Problem("An error occured while deleting the user", statusCode: 500);
      }
      
      return Results.NoContent();
   }
}