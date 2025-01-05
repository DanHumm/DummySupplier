namespace Logistics_Supplier1_API.DTOs.User;

/// <summary>
/// Represents a request to get all users.
/// </summary>
public class GetAllUsersRequest
{
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the role of the user.
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// Gets or sets the address of the user.
    /// </summary>
    public string Address { get; set; }
}