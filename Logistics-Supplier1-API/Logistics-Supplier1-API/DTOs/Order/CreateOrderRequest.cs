using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.DTOs.Order;

/// <summary>
/// Represents a request to create a new order.
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// Gets or sets the list of order items in the new order.
    /// </summary>
    [Required]
    public List<OrderItemDto> OrderItems { get; set; }
}

/// <summary>
/// Represents an item within an order in a create order request.
/// </summary>
public class OrderItemDto
{
    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit) of the product.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Sku { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    
}