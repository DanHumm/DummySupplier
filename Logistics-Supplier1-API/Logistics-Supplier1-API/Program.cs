using System.Text;
using dotenv.net;
using Logistics_Supplier1_API.Data;
using Logistics_Supplier1_API.Handlers;
using Logistics_Supplier1_API.Helpers;
using Logistics_Supplier1_API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
DotEnv.Load();

builder.WebHost.UseKestrel();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps("./https/server.pfx", Environment.GetEnvironmentVariable("PFX_PASSWORD"));
    });
});

var connectionString = "Data Source=encrypted-supplier.db;Password="+Environment.GetEnvironmentVariable("DB_PASSWORD");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.RequireHttpsMetadata = false;
        options.UseSecurityTokenValidators = true;
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(context.Exception.Message);
                Console.WriteLine(context.Exception.StackTrace);
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", ""); 
                Console.WriteLine($"Received Token: {token ?? "null"}"); 
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY"))),
            ValidIssuer = Environment.GetEnvironmentVariable("JWT_SECRET_ISSUER"),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(swag =>
{
    swag.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "[DEV] DairyDoughSupplies API",
        Version = "v1",
        Description = "Development version of the DairyDoughSupplies API. Should you require a production version of the application, please raise an issue on the Logistics Management Github Repository or contact me directly via <a href='mailto:s5416877@bournemouth.ac.uk'>s5416877@bournemouth.ac.uk</a>",
    });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    swag.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    swag.AddSecurityRequirement(securityRequirement);
});
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IInviteCodeRepository, InviteCodeRepository>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger(); // ONLY PRESENT IN DEV PACKAGE
app.UseSwaggerUI(); // ONLY PRESENT IN DEV PACKAGE

app.UseHttpsRedirection();

// Adds Security Headers to Responses
app.Use(async (context, next) =>
{

    context.Response.Headers.Add("Cache-Control", "no-store, max-age=0");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; form-action 'self'; object-src 'none'; frame-ancestors 'none'; upgrade-insecure-requests; block-all-mixed-content");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("Cross-Origin-Resource-Policy", "same-origin");
    context.Response.Headers.Add("Cross-Origin-Embedder-Policy", "same-origin");
    context.Response.Headers.Add("Cross-Origin-Opener-Policy", "same-origin");

    if (context.Request.ContentLength > 0 &&
        context.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
    {
        try
        {



            context.Request.EnableBuffering();
            string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (!JsonValidator.IsValidJson(requestBody))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                    { error = "Invalid or malformed Json. Please validate your request and try again." });
                return;
            }

        }
        catch (Exception e)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
                { error = "Invalid or malformed Json. Please validate your request and try again." });
            return;
        }
    }
    
    await next(context); 
});

app.MapGet("/", () =>
{
    var response = new Dictionary<string, object>
    {
        { "message", "Welcome to the Logistics Supplier API!" },
        { "version", "1.0.0" },
        { "documentation", "/swagger" },
        {
            "Notes",
            "This is a development environment, therefore documentation is openly accessible. If you require a production version of the application, please raise an issue on the Logistics Management Github Repository or contact me directly via s5416877@bournemouth.ac.uk"
        },
    };

    return Results.Json(response); // Return the response as JSON
});

// ----------------------------------------- Auth Endpoints -----------------------------------

/// <summary>
/// Registers a new user.
/// </summary>
app.MapPost("/api/register", RegistrationHandler.RegisterUser);

/// <summary>
/// Authenticates a user and returns a JWT token.
/// </summary>
app.MapPost("/api/login", AuthenticationHandler.Authenticate);

// ----------------------------------------- Order Endpoints -----------------------------------

/// <summary>
/// Creates a new order.
/// </summary>
app.MapPost("/api/orders/create", OrderHandlers.CreateOrder).RequireAuthorization();

/// <summary>
/// Gets all orders (for the current user or all orders if the user is an admin).
/// </summary>
app.MapGet("/api/orders", OrderHandlers.GetAllOrders).RequireAuthorization();

/// <summary>
/// Gets an order by its ID.
/// </summary>
app.MapGet("/api/orders/{orderid}", OrderHandlers.GetOrderById).RequireAuthorization();

/// <summary>
/// Updates an existing order.
/// </summary>
app.MapPut("/api/orders/{orderId}", OrderHandlers.UpdateOrderAsync).RequireAuthorization();

/// <summary>
/// Deletes an order.
/// </summary>
app.MapDelete("/api/orders/{orderid}", OrderHandlers.DeleteOrderAsync).RequireAuthorization();

// ----------------------------------------- Product Endpoints -----------------------------------

/// <summary>
/// Gets all products.
/// </summary>
app.MapGet("/api/products", ProductHandlers.GetAllProducts).RequireAuthorization();

/// <summary>
/// Gets a product by its SKU.
/// </summary>
app.MapGet("/api/product/{productSku}", ProductHandlers.GetProductBySkuAsync).RequireAuthorization();

/// <summary>
/// Creates a new product.
/// </summary>
app.MapPost("/api/products", ProductHandlers.CreateProductAsync).RequireAuthorization();

/// <summary>
/// Updates an existing product.
/// </summary>
app.MapPut("/api/product/", ProductHandlers.UpdateProductAsync).RequireAuthorization();

/// <summary>
/// Deletes a product.
/// </summary>
app.MapDelete("/api/product/{productSku}", ProductHandlers.DeleteProductAsync).RequireAuthorization();

// ----------------------------------------- User Endpoints -----------------------------------

/// <summary>
/// Gets all users.
/// </summary>
app.MapGet("/api/users", UserHandlers.GetAllUsersAsync).RequireAuthorization();

/// <summary>
/// Gets a user by their username.
/// </summary>
app.MapGet("/api/user/{username}", UserHandlers.GetUserByUsernameAsync).RequireAuthorization();

/// <summary>
/// Updates an existing user.
/// </summary>
app.MapPut("/api/user/{username}", UserHandlers.UpdateUserAsync).RequireAuthorization();

/// <summary>
/// Deletes a user.
/// </summary>
app.MapDelete("/api/user/{username}", UserHandlers.DeleteUserAsync).RequireAuthorization();

// ----------------------------------------- Invite Code Endpoints -----------------------------------

/// <summary>
/// Gets all invite codes.
/// </summary>
app.MapGet("/api/invitecodes", InviteCodeHandler.GetAllInviteCodes).RequireAuthorization();

/// <summary>
/// Generates a new invite code.
/// </summary>
app.MapGet("/api/invitecodes/generate", InviteCodeHandler.GenerateInviteCode).RequireAuthorization();

app.Run();


