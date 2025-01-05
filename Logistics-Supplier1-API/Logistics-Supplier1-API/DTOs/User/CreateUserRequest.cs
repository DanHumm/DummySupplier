using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.DTOs.User;

/// <summary>
/// Represents a request to create a new user.
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// Gets or sets the username for the new user.
    /// </summary>
    [Required]
    [MinLength(3)]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password for the new user.
    /// </summary>
    [MinLength(8)]
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the address for the new user.
    /// </summary>
    [Required]
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the invite code for registration.
    /// </summary>
    public string inviteCode { get; set; }
}