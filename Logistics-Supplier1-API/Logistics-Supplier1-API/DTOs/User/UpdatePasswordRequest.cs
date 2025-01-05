using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.DTOs.User;

/// <summary>
/// Represents a request to update a user's password.
/// </summary>
public class UpdatePasswordRequest
{
    /// <summary>
    /// Gets or sets the user's current password.
    /// </summary>
    [Required]
    [MinLength(8)]
    public string CurrentPassword { get; set; }

    /// <summary>
    /// Gets or sets the user's new password.
    /// </summary>
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; }
}