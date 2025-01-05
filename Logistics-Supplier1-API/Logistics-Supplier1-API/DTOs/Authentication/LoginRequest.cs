using System.ComponentModel.DataAnnotations;
using Logistics_Supplier1_API.DTOs.User;

namespace Logistics_Supplier1_API.DTOs.Authentication;

/// <summary>
/// Represents a request to log in a user.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    [Required]
    public string Password { get; set; }
}