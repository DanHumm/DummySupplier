using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.DTOs.User;

/// <summary>
/// Represents a request to get a specific user.
/// </summary>
public class GetUserRequest
{
    /// <summary>
    /// Gets or sets the username of the user to retrieve.
    /// </summary>
    [Required]
    public string Username { get; set; }
}