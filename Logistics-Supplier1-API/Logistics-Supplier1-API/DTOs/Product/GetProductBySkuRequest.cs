using System.ComponentModel.DataAnnotations;

namespace Logistics_Supplier1_API.DTOs.Product;

/// <summary>
/// Represents a request to get a product by its SKU.
/// </summary>
public class GetProductBySkuRequest
{
    /// <summary>
    /// Gets or sets the SKU of the product to retrieve.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Sku { get; set; }
}