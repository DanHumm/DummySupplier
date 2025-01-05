namespace Logistics_Supplier1_API.Data;
using Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents the database context for the application.
/// </summary>
public class MyDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for configuring the database context.</param>
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }
    
    /// <summary>
    /// Gets or sets the DbSet for User entities.
    /// </summary>
    public DbSet<User> Users { get; set; }
    
    /// <summary>
    /// Gets or sets the DbSet for Order entities.
    /// </summary>
    public DbSet<Order> Orders { get; set; }
    
    /// <summary>
    /// Gets or sets the DbSet for Product entities.
    /// </summary>
    public DbSet<Product> Products { get; set; }
    
    /// <summary>
    /// Gets or sets the DbSet for OrderItem entities.
    /// </summary>
    public DbSet<OrderItem> OrderItems { get; set; }
    
    /// <summary>
    /// Gets or sets the DbSet for InviteCode entities.
    /// </summary>
    public DbSet<InviteCode> InviteCodes { get; set; }

    /// <summary>
    /// Configures the model for the database context.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}