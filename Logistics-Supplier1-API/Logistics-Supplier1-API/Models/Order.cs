using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.Models;

/// <summary>
/// Represents an order in the system.
/// </summary>
public class Order
{
    /// <summary>
    /// Gets or sets the unique identifier of the order.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets or sets the date and time when the order was placed.
    /// </summary>
    public DateTime OrderDate { get; private set; }

    /// <summary>
    /// Gets or sets the date and time when the order was last updated.
    /// </summary>
    public DateTime LastUpdate { get; private set; }

    /// <summary>
    /// Gets or sets the ID of the user who placed the order.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the list of order items associated with the order.
    /// </summary>
    [Required]
    public List<OrderItem> OrderItems { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="userId">The ID of the user who placed the order.</param>
    /// <param name="orderItems">The list of order items.</param>
    public Order(int userId, List<OrderItem> orderItems)
    {
        OrderDate = DateTime.UtcNow;
        UserId = userId;
        OrderItems = orderItems;
    }

    /// <summary>
    /// Updates the order with new details.
    /// </summary>
    /// <param name="id">The ID of the order.</param>
    /// <param name="orderItems">The new list of order items.</param>
    /// <returns>The updated order instance.</returns>
    public Order updateOrder(int id, List<OrderItem> orderItems)
    {
        LastUpdate = DateTime.UtcNow;
        Id = id;
        OrderItems = orderItems;

        return this;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class (required for Entity Framework Core).
    /// </summary>
    public Order()
    {
    }
}

/// <summary>
/// Represents an item within an order.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Gets or sets the unique identifier of the order item.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit) of the product in the order item.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ProductSku { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product in the order item.
    /// </summary>
    [Required]
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product in the order item.
    /// </summary>
    [Required]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderItem"/> class.
    /// </summary>
    /// <param name="productSku">The SKU of the product.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="price">The unit price of the product.</param>
    public OrderItem(string productSku, int quantity, decimal price)
    {
        ProductSku = productSku;
        Quantity = quantity;
        UnitPrice = price;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderItem"/> class (required for Entity Framework Core).
    /// </summary>
    public OrderItem()
    {
    }
}