using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.DTOs.Order;

/// <summary>
/// Represents a request to delete an order.
/// </summary>
public class DeleteOrderRequest
{
    /// <summary>
    /// Gets or sets the ID of the order to delete.
    /// </summary>
    [Required]
    public int OrderId { get; set; }
}