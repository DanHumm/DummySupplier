using Logistics_Supplier1_API.DTOs.User;
using Logistics_Supplier1_API.Models;

namespace Logistics_Supplier1_API.Repositories;

// IUserRepository.cs

/// <summary>
/// Defines methods for interacting with User data.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by their username asynchronously.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the user if found, otherwise null.</returns>
    Task<User> GetUserByUsernameAsync(string username);

    /// <summary>
    /// Gets all users asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of all users.</returns>
    Task<List<GetAllUsersRequest>> GetAllUsersAsync();

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created user.</returns>
    Task<User> CreateUserAsync(User user);

    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated user.</returns>
    Task<User> UpdateUserAsync(User user);

    /// <summary>
    /// Deletes a user asynchronously.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted user.</returns>
    Task<User> DeleteUserAsync(User user);
}