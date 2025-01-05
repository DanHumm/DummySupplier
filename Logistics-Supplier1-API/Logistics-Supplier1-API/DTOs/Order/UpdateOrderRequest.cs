using System.ComponentModel.DataAnnotations;
using Logistics_Supplier1_API.Models;

namespace Logistics_Supplier1_API.DTOs.Order;

/// <summary>
/// Represents a request to update an order.
/// </summary>
public class UpdateOrderRequest
{
    /// <summary>
    /// Gets or sets the ID of the order to update.
    /// </summary>
    [Required]
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the list of order items in the updated order.
    /// </summary>
    [Required]
    public List<OrderItem> OrderItems { get; private set; }
}