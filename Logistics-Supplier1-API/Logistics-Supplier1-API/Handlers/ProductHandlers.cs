using Logistics_Supplier1_API.DTOs.Product;
using Logistics_Supplier1_API.Models;
using Logistics_Supplier1_API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logistics_Supplier1_API.Handlers;

/// <summary>
/// Handles API requests related to products.
/// </summary>
public class ProductHandlers
{
    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <param name="productRepository">The repository for accessing product data.</param>
    /// <returns>A list of all products.</returns>
    public static async Task<IResult> GetAllProducts(IProductRepository productRepository)
    {
        var products = await productRepository.GetAllProductsAsync();
        return Results.Ok(products);
    }

    /// <summary>
    /// Gets a product by its SKU.
    /// </summary>
    /// <param name="sku">The SKU of the product.</param>
    /// <param name="productRepository">The repository for accessing product data.</param>
    /// <returns>The product with the specified SKU.</returns>
    public static async Task<IResult> GetProductBySkuAsync(string productSku, IProductRepository productRepository)
    {
        Console.WriteLine($"Sku: {productSku}");
        var product = await productRepository.GetProductBySkuAsync(productSku);
        if (product == null)
        {
            return Results.NotFound("No product found with that SKU");
        }
        return Results.Ok(product);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productRepository">The repository for accessing product data.</param>
    /// <param name="req">The create product request.</param>
    /// <returns>The created product.</returns>
    [Authorize(Roles = "admin")]
    public static async Task<IResult> CreateProductAsync(
        IProductRepository productRepository,
        [FromBody] CreateProductRequest req = null)
    {
        if (req == null)
        {
            return Results.BadRequest("No request body was provided");
        }
        if (string.IsNullOrEmpty(req.Sku))
        {
            return Results.BadRequest("A valid sku is required. e.g. MLK001");
        } 
        else if (string.IsNullOrWhiteSpace(req.Name))
        {
            return Results.BadRequest("A valid name is required. e.g. JPs Whole Milk 2LTR");
        }
        else if (!decimal.TryParse(req.Price.ToString(), out decimal price))
        {
            return Results.BadRequest("A valid price is required. e.g. 10.99 or 10.00");
        }
        else if (req.QuantityInStock == null)
        {
            return Results.BadRequest("A valid stock quantity is required. e.g. 1");
        }
        var product = new Product(req.Sku, req.Name, req.Price, req.QuantityInStock);
        try
        {
            await productRepository.CreateProductAsync(product);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occured while adding product: {ex.Message}");
        }
        
        return Results.Created("/api/products/{product.sku}", product);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="sku">The SKU of the product to update.</param>
    /// <param name="productRepository">The repository for accessing product data.</param>
    /// <param name="req">The update product request.</param>
    /// <returns>The updated product.</returns>
    [Authorize(Roles = "admin")]
    public static async Task<IResult> UpdateProductAsync(
        IProductRepository productRepository,
        [FromBody] List<UpdateProductRequest> reqs = null)
    {
        if (reqs == null || reqs.Count == 0)
        {
            return Results.BadRequest("No request body was provided");
        }
        
        var updatedProducts = new List<Product>();

        foreach (var req in reqs)
        {
            if (string.IsNullOrEmpty(req.Sku))
            {
                return Results.BadRequest("A valid sku is required. e.g. MLK001");
            }
            else if (string.IsNullOrWhiteSpace(req.Name))
            {
                return Results.BadRequest($"A valid name for sku {req.Sku} is required. e.g. JPs Whole Milk 2LTR");
            }
            else if (!decimal.TryParse(req.Price.ToString(), out decimal price))
            {
                return Results.BadRequest($"A valid price for sku {req.Sku} is required. e.g. 10.99 or 10.00");
            }
            else if (req.StockQuantity == null)
            {
                return Results.BadRequest($"A valid stock quantity is required for sku {req.Sku}. e.g. 1");
            }
            
            var product = await productRepository.GetProductBySkuAsync(req.Sku);

            if (product == null)
            {
                return Results.NotFound($"Product with sku: {req.Sku} not found");
            }

            product.UpdateProduct(req.Name, req.Price, req.StockQuantity);
            try
            {
                await productRepository.UpdateProductAsync(product);
                updatedProducts.Add(product);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        return Results.Ok(updatedProducts);
    }

    /// <summary>
    /// Deletes a product.
    /// </summary>
    /// <param name="sku">The SKU of the product to delete.</param>
    /// <param name="productRepository">The repository for accessing product data.</param>
    /// <returns>A result indicating whether the product was deleted successfully.</returns>
    [Authorize(Roles = "admin")]
    public static async Task<IResult> DeleteProductAsync(string sku, IProductRepository productRepository)
    {
        var product = await productRepository.GetProductBySkuAsync(sku);
        if (product == null)
        {
            return Results.NotFound("Product not found");
        }

        try
        {
            await productRepository.DeleteProductAsync(product);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }

        return Results.NoContent();
    }
}