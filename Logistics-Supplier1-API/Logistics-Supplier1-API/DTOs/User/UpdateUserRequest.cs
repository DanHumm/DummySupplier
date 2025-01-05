using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.DTOs.User;

/// <summary>
/// Represents a request to update a user's information.
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// Gets or sets the new role for the user (optional).
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Gets or sets the new address for the user (optional).
    /// </summary>
    public string? Address { get; set; }
}