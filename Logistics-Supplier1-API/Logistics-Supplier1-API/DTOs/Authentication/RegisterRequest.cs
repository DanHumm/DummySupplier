using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.DTOs.Authentication;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 8)]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the invite code for registration.
    /// </summary>
    [Required]
    [StringLength(64, MinimumLength = 8)]
    public string inviteCode { get; set; }

    /// <summary>
    /// Gets or sets the address of the user.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Address { get; set; }
}