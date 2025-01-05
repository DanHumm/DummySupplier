using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.Models;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    [MaxLength(100)]
    public int Id { get; private set; }

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(30)]
    public string Username { get; private set; }

    /// <summary>
    /// Gets or sets the hashed password of the user.
    /// </summary>
    [Required]
    public string PasswordHash { get; private set; }

    /// <summary>
    /// Gets or sets the role of the user.
    /// </summary>
    [Required]
    [MaxLength(15)]
    public string Role { get; private set; }

    /// <summary>
    /// Gets or sets the address of the user.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Address { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="passwordHash">The hashed password of the user.</param>
    /// <param name="role">The role of the user.</param>
    /// <param name="address">The address of the user.</param>
    public User(string username, string passwordHash, string role, string address)
    {
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
        Address = address;
    }

    /// <summary>
    /// Updates the user's information.
    /// </summary>
    /// <param name="role">The new role of the user (optional).</param>
    /// <param name="address">The new address of the user (optional).</param>
    public void UpdateUser(string role = null, string address = null)
    {
        if (role != null)
        {
            Role = role;
        }

        if (address != null)
        {
            Address = address;
        }
    }
}