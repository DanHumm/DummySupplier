using System.Security.Claims;
using Logistics_Supplier1_API.DTOs.Order;
using Logistics_Supplier1_API.Models;
using Logistics_Supplier1_API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logistics_Supplier1_API.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Logistics_Supplier1_API.Handlers;

/// <summary>
/// Handles API requests related to orders.
/// </summary>
public class OrderHandlers
{
    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="orderRepository">The repository for accessing order data.</param>
    /// <param name="productRepository">The repository for accessing product data.</param>
    /// <param name="httpContext">The HTTP context of the request.</param>
    /// <param name="req">The create order request.</param>
    /// <returns>The created order.</returns>
    [Authorize(Roles = "user,admin", AuthenticationSchemes = "Bearer")]
    public static async Task<IResult> CreateOrder(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        HttpContext httpContext,
        [FromBody] CreateOrderRequest req = null)
    {
        if (req == null || req.OrderItems == null || req.OrderItems.Count == 0)
        {
            return Results.Json("No request body was found or there were no order items provided.", statusCode: 400);
        }

        var userIdClaim = JwtHelper.GetUserIdFromJwt(httpContext);
        if (userIdClaim == null)
        {
            return Results.Json(new { message = "An error occured. Please sign in again." },
                statusCode: 401);
        }

        if (req.OrderItems == null || req.OrderItems.Count == 0)
        {
            return Results.BadRequest("Order items are required");
        }

        var orderItems = new List<OrderItem>();
        foreach (var item in req.OrderItems)
        {

            var product = await productRepository.GetProductBySkuAsync(item.Sku);
            if (product == null)
            {
                return Results.BadRequest($"Product was not found ({item.Sku}). Please check your SKU and try again.");
            } 
            orderItems.Add(new OrderItem(item.Sku, item.Quantity, product.Price));
        }
        
        var stockOrderItems = new List<OrderItem>();
        foreach (var item in req.OrderItems)
        {
            var product = await productRepository.GetProductBySkuAsync(item.Sku);
            if (product == null)
            {
                return Results.BadRequest($"Product was not found ({item.Sku})");
            }
            stockOrderItems.Add(new OrderItem(item.Sku, item.Quantity, product.Price));
        }

        var result = await CheckStockForOrderItems(productRepository, stockOrderItems);
        if (result is not Ok)
        {
            return result;
        }
        
        var order = new Order(userIdClaim.GetValueOrDefault(), orderItems);
        int orderId = -1;
        try
        {
            orderId = await orderRepository.CreateOrderAsync(order);
        }
        catch (Exception e)
        {
            Results.BadRequest(e.Message);
        }

        if (orderId == -1)
        {
            return Results.BadRequest("Order was not created, an error occured.");
        }
        return Results.Created($"/api/orders/{orderId}", order);
    }

    /// <summary>
    /// Gets an order by its ID.
    /// </summary>
    /// <param name="orderId">The ID of the order.</param>
    /// <param name="orderRepository">The repository for accessing order data.</param>
    /// <param name="httpContext">The HTTP context of the request.</param>
    /// <returns>The order with the specified ID.</returns>
    [Authorize(Roles = "user,admin", AuthenticationSchemes = "Bearer")]
    public static async Task<IResult> GetOrderById(int orderId, IOrderRepository orderRepository, HttpContext httpContext)
    {
        var userIdClaim = JwtHelper.GetUserIdFromJwt(httpContext);
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order.UserId != userIdClaim.GetValueOrDefault())
        {
            return Results.Unauthorized();
        }
        else if (order.OrderItems == null || order.OrderItems.Count == 0)
        {
            return Results.NotFound("Order was not found. Please check your order ID and try again.");
        }
        else
        {
            return Results.Ok(order);
        }
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="orderId">The ID of the order to update.</param>
    /// <param name="orderRepository">The repository for accessing order data.</param>
    /// <param name="httpContext">The HTTP context of the request.</param>
    /// <param name="productRepository">The repository for accessing product data.</param>
    /// <param name="req">The update order request.</param>
    /// <returns>The updated order.</returns>
    [Authorize(Roles = "user,admin", AuthenticationSchemes = "Bearer")]
    public static async Task<IResult> UpdateOrderAsync(int orderId,
        IOrderRepository orderRepository,
        HttpContext httpContext,
        IProductRepository productRepository,
        [FromBody] UpdateOrderRequest req = null)
    {

        if (req == null || req.OrderItems == null || req.OrderItems.Count == 0)

        {
            return Results.Json("No request body was found or there were no order items provided.", statusCode: 400);
        }
        var userIdClaim = JwtHelper.GetUserIdFromJwt(httpContext);
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order.UserId != userIdClaim.GetValueOrDefault() || JwtHelper.GetRoleFromJwt(httpContext) != "Admin")
        {
            return Results.Unauthorized();
        }
        else if (order.OrderItems == null || order.OrderItems.Count == 0)
        {
            return Results.NotFound("Order was not found. Please check your order ID and try again.");
        }
        else
        {
            try
            {
                await CheckStockForOrderItems(productRepository, order.OrderItems);
                order.updateOrder(order.Id, req.OrderItems);
                await orderRepository.UpdateOrderAsync(order);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }

            return Results.Ok(order);
        }
    }

    /// <summary>
    /// Deletes an order.
    /// </summary>
    /// <param name="orderId">The ID of the order to delete.</param>
    /// <param name="orderRepository">The repository for accessing order data.</param>
    /// <param name="httpContext">The HTTP context of the request.</param>
    /// <returns>A result indicating whether the order was deleted successfully.</returns>
    [Authorize(Roles = "user,admin", AuthenticationSchemes = "Bearer")]
    public static async Task<IResult> DeleteOrderAsync(int orderId, IOrderRepository orderRepository,
        HttpContext httpContext)
    {
        var userIdClaim = JwtHelper.GetUserIdFromJwt(httpContext);
        var order = await orderRepository.GetOrderByIdAsync(orderId);

        if (order.UserId != userIdClaim.GetValueOrDefault() || JwtHelper.GetRoleFromJwt(httpContext) != "Admin")
        {
            return Results.Unauthorized();
        }
        else
        {
            if (order.OrderItems == null || order.OrderItems.Count == 0)
            {
                return Results.NotFound("Order was not found. Please check your order ID and try again.");
            }
            else
            {
                try
                {
                    await orderRepository.DeleteOrderAsync(order);
                }
                catch (Exception e)
                {
                    return Results.Problem(e.Message);
                }
            }
        }

        return Results.Json("Successfully deleted order.", statusCode: 204);
    }

    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <param name="orderRepository">The repository for accessing order data.</param>
    /// <param name="httpContext">The HTTP context of the request.</param>
    /// <returns>A list of all orders.</returns>
    [Authorize]
    public static async Task<IResult> GetAllOrders(
        IOrderRepository orderRepository,
        HttpContext httpContext)
    {
        List<Order> orders;
        var userIdClaim = JwtHelper.GetUserIdFromJwt(httpContext);
        Console.WriteLine(httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier));
        Console.WriteLine("\n" + httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role));
        var userRoleClaim = JwtHelper.GetRoleFromJwt(httpContext);
        if (userIdClaim != null && userRoleClaim != null)
        {
            if (userRoleClaim == "admin")
            {
                orders = await orderRepository.GetAllOrdersAsync();
            }
            else
            {
                orders = await orderRepository.GetOrdersByUserIdAsync(userIdClaim.Value);
            }

        }
        else
        {
            return Results.BadRequest();
        }

        return Results.Ok(orders);
    }

    /// <summary>
    /// Checks if there is enough stock to fulfill the given order items.
    /// </summary>
    /// <param name="orderItems">The list of order items to check.</param>
    /// <param name="productRepository">The repository for accessing product data.</param>
    /// <param name="existingOrderItems">The existing order items (optional).</param>
    /// <returns>A result indicating whether there is enough stock.</returns>
    private static async Task<IResult> CheckStockForOrderItems(
        IProductRepository productRepository,
        List<OrderItem> existingOrderItems = null)
    {
        Console.WriteLine("INSIDE CHECK STOCK ORDER");
        var products = await productRepository.GetAllProductsAsync();
        Dictionary<string, OrderItem> orderItemMap = null;
        if (existingOrderItems == null)
        {
            return Results.BadRequest("Cannot find existing order items.");
        } else if (products == null || products.Count == 0)
        {
            return Results.BadRequest("Cannot find products list");
        }
        orderItemMap = existingOrderItems.ToDictionary(item => item.ProductSku, item => item);
        
        var insufficientStock = new List<string>();
        foreach (var item in products)
        {
            int stockChange;
            if (orderItemMap.ContainsKey(item.Sku))
            {
              OrderItem matchingItem = orderItemMap[item.Sku];
              stockChange = item.StockQuantity - matchingItem.Quantity;
              
              try
              {
                  Console.WriteLine("ATTEMPTING TO DECREASE STOCK");

                  item.DecreaseStockQuantity(stockChange);
              }
              catch (InvalidOperationException)
              {
                  insufficientStock.Add($"{item.Sku} - Requested: {stockChange}, Stock level: {item.StockQuantity}\\n");
              }
            }
        }

        if (insufficientStock.Count > 0)
        {
            var errorMessage = "Sorry, we do not have enough stock to fulfill the following items:" + insufficientStock;
            return Results.BadRequest(errorMessage);
        }

        return Results.Ok();
    }
}