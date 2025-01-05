using Logistics_Supplier1_API.Data;
using Logistics_Supplier1_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Supplier1_API.Repositories;

/// <summary>
/// Provides methods for interacting with Order data in the database.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly MyDbContext _context;

    /// <summary>
    /// Initializes a new instance of the OrderRepository class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public OrderRepository(MyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets an order by its ID asynchronously.
    /// </summary>
    /// <param name="orderId">The ID of the order to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the order if found, otherwise null.</returns>
    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id.ToString() == orderId.ToString());
    }

    /// <summary>
    /// Gets all orders asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of all orders.</returns>
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all orders for a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of orders for the specified user.</returns>
    public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    /// <summary>
    /// Creates a new order asynchronously.
    /// </summary>
    /// <param name="order">The order to create.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the ID of the created order.</returns>
    public async Task<int> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order.Id;
    }

    /// <summary>
    /// Updates an existing order asynchronously.
    /// </summary>
    /// <param name="order">The order to update.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated order.</returns>
    public async Task<Order> UpdateOrderAsync(Order order)
    {
        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return order;
    }

    /// <summary>
    /// Deletes an order asynchronously.
    /// </summary>
    /// <param name="order">The order to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted order.</returns>
    public async Task<Order> DeleteOrderAsync(Order order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return order;
    }
}