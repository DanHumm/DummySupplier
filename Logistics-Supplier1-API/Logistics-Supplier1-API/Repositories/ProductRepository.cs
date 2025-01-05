using Logistics_Supplier1_API.Models;
using Logistics_Supplier1_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Supplier1_API.Repositories;

/// <summary>
/// Provides methods for interacting with Product data in the database.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly MyDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ProductRepository class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ProductRepository(MyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all products asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of all products.</returns>
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    /// <summary>
    /// Gets a product by its SKU asynchronously.
    /// </summary>
    /// <param name="sku">The SKU of the product to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the product if found, otherwise null.</returns>
    public async Task<Product> GetProductBySkuAsync(string sku)
    {
        return await _context.Products.FirstOrDefaultAsync(p => string.Equals(p.Sku, sku));
    }

    /// <summary>
    /// Creates a new product asynchronously.
    /// </summary>
    /// <param name="product">The product to create.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created product.</returns>
    public async Task<Product> CreateProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Updates an existing product asynchronously.
    /// </summary>
    /// <param name="product">The product to update.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated product.</returns>
    public async Task<Product> UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Deletes a product asynchronously.
    /// </summary>
    /// <param name="product">The product to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted product.</returns>
    public async Task<Product> DeleteProductAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return product;
    }
}