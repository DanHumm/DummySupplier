using Logistics_Supplier1_API.Models;

namespace Logistics_Supplier1_API.Repositories;

// IOrderRepository.cs

/// <summary>
/// Defines methods for interacting with Order data.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Gets all orders asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of all orders.</returns>
    Task<List<Order>> GetAllOrdersAsync();
    
    /// <summary>
    /// Gets an order by its ID asynchronously.
    /// </summary>
    /// <param name="orderId">The ID of the order to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the order if found, otherwise null.</returns>
    Task<Order> GetOrderByIdAsync(int orderId);

    /// <summary>
    /// Creates a new order asynchronously.
    /// </summary>
    /// <param name="order">The order to create.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the ID of the created order.</returns>
    Task<int> CreateOrderAsync(Order order);

    /// <summary>
    /// Updates an existing order asynchronously.
    /// </summary>
    /// <param name="order">The order to update.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated order.</returns>
    Task<Order> UpdateOrderAsync(Order order);

    /// <summary>
    /// Deletes an order asynchronously.
    /// </summary>
    /// <param name="order">The order to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted order.</returns>
    Task<Order> DeleteOrderAsync(Order order);
    

    /// <summary>
    /// Gets all orders for a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of orders for the specified user.</returns>
    Task<List<Order>> GetOrdersByUserIdAsync(int userId);
}