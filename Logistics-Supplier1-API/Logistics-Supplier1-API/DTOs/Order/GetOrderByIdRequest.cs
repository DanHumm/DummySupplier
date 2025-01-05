namespace Logistics_Supplier1_API.DTOs.Order;

/// <summary>
/// Represents a request to get an order by its ID.
/// </summary>
public class GetOrderByIdRequest
{
    /// <summary>
    /// Gets or sets the ID of the order.
    /// </summary>
    public int OrderId { get; set; }
}