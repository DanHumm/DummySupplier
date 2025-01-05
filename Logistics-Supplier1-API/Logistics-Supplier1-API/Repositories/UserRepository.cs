using Logistics_Supplier1_API.Data;
using Logistics_Supplier1_API.DTOs.User;
using Logistics_Supplier1_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Supplier1_API.Repositories;

/// <summary>
/// Provides methods for interacting with User data in the database.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly MyDbContext _dbcontext;

    /// <summary>
    /// Initializes a new instance of the UserRepository class.
    /// </summary>
    /// <param name="dbcontext">The database context.</param>
    public UserRepository(MyDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    /// <summary>
    /// Gets a user by their username asynchronously.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the user if found, otherwise null.</returns>
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _dbcontext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    /// <summary>
    /// Gets all users asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of all users.</returns>
    public async Task<List<GetAllUsersRequest>> GetAllUsersAsync()
    {
        return await _dbcontext.Users.Select(u => new GetAllUsersRequest
        {
            Id = u.Id,
            Username = u.Username,
            Role = u.Role,
            Address = u.Address
        }).ToListAsync();
    }

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created user.</returns>
    public async Task<User> CreateUserAsync(User user)
    {
        _dbcontext.Users.Add(user);
        await _dbcontext.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated user.</returns>
    public async Task<User> UpdateUserAsync(User user)
    {
        _dbcontext.Users.Update(user);
        await _dbcontext.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Deletes a user asynchronously.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted user.</returns>
    public async Task<User> DeleteUserAsync(User user)
    {
        _dbcontext.Users.Remove(user);
        await _dbcontext.SaveChangesAsync();
        return user;
    }
}