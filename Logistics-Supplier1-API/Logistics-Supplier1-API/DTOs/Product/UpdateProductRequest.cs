using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.DTOs.Product;

/// <summary>
/// Represents a request to update a product.
/// </summary>
public class UpdateProductRequest
{
    /// <summary>
    /// Gets or sets the SKU of the product to update.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Sku { get; set; }

    /// <summary>
    /// Gets or sets the new name of the product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the new price of the product.
    /// </summary>
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the new stock quantity of the product.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
}