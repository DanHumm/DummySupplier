namespace Logistics_Supplier1_API.Repositories;
using Logistics_Supplier1_API.Models;

// IProductRepository.cs

/// <summary>
/// Defines methods for interacting with Product data.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Gets a product by its SKU asynchronously.
    /// </summary>
    /// <param name="sku">The SKU of the product to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the product if found, otherwise null.</returns>
    Task<Product> GetProductBySkuAsync(string sku);

    /// <summary>
    /// Gets all products asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of all products.</returns>
    Task<List<Product>> GetAllProductsAsync();

    /// <summary>
    /// Creates a new product asynchronously.
    /// </summary>
    /// <param name="product">The product to create.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created product.</returns>
    Task<Product> CreateProductAsync(Product product);

    /// <summary>
    /// Updates an existing product asynchronously.
    /// </summary>
    /// <param name="product">The product to update.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated product.</returns>
    Task<Product> UpdateProductAsync(Product product);

    /// <summary>
    /// Deletes a product asynchronously.
    /// </summary>
    /// <param name="product">The product to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted product.</returns>
    Task<Product> DeleteProductAsync(Product product);
}