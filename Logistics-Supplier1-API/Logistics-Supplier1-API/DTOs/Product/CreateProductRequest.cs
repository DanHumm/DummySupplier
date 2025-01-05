using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.DTOs.Product;

/// <summary>
/// Represents a request to create a new product.
/// </summary>
public class CreateProductRequest
{
    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit) of the product.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Sku { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue)] // Ensure the price is positive
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the initial stock quantity of the product.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)] // Ensure quantity is non-negative
    public int QuantityInStock { get; set; }
    
}