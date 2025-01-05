using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.Models;

/// <summary>
/// Represents a product in the system.
/// </summary>
public class Product
{
    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit) of the product.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Sku { get; private set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; private set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)] // Ensure the price is positive
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets or sets the stock quantity of the product.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="sku">The SKU of the product.</param>
    /// <param name="name">The name of the product.</param>
    /// <param name="price">The price of the product.</param>
    /// <param name="stockQuantity">The stock quantity of the product.</param>
    public Product(string sku, string name, decimal price, int stockQuantity)
    {
        Sku = sku;
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }

    /// <summary>
    /// Decreases the stock quantity of the product.
    /// </summary>
    /// <param name="quantity">The quantity to decrease by.</param>
    /// <exception cref="ArgumentException">Thrown when the quantity is not positive or is greater than the current stock quantity.</exception>
    public void DecreaseStockQuantity(int quantity)
    {
        if (quantity > 0 && quantity <= StockQuantity)
        {
            StockQuantity = StockQuantity - quantity;
        }
        else if (quantity <= 0)
        {
            throw new InvalidOperationException("Quantity must be positive");
        }
        else
        {
            throw new InvalidOperationException("Quantity cannot exceed the current stock quantity.");
        }
    }

    /// <summary>
    /// Updates the product's information.
    /// </summary>
    /// <param name="name">The new name of the product.</param>
    /// <param name="price">The new price of the product.</param>
    /// <param name="stockQuantity">The new stock quantity of the product.</param>
    public void UpdateProduct(string name, decimal price, int stockQuantity)
    {
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }
}