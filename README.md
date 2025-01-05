# DummySupplier: A RESTful API for a fake supplier

This project is an ASP.NET Core Minimal web API that simulates a supplier system. It's created to be a learning experience for myself, focusing on building my first secure RESTful API using some modern .NET technologies.

## Key Features

* **RESTful API Design:**  Adheres to REST principles for predictable and consistent API endpoints.
* **Entity Framework Core:**  Uses EF Core for data access and database interactions (`Microsoft.EntityFrameworkCore`).
* **Role-Based Access Control (RBAC):**  Implements RBAC using JWTs (JSON Web Tokens) (`Microsoft.AspNetCore.Authentication.JwtBearer`, `Microsoft.IdentityModel.Tokens`) to restrict access to resources based on user roles.
* **Secure Coding Practices:**  Implements good security practices to prevent common vulnerabilities (e.g., using HTTPS, secure password hashing, OWASP Recommended HTTP security headers).
* **Swagger/OpenAPI Documentation:**  Includes Swagger UI (`Microsoft.OpenApi.Models`) for easy API exploration and documentation.
* **Dockerized:**  Provides a `Dockerfile` for containerized deployment with automated package builds using Github Actions.
* **Environment Variables:**  Uses `dotenv.net` to manage environment variables for configuration and secrets.

## Technologies Used

* .NET 8
* Entity Framework Core
* JWT (JSON Web Tokens)
* Swagger/OpenAPI
* Docker

## Project Structure

* **Logistics-Supplier1-API:**
    * **Data:** 
        * `DbContext.cs`:  Sets up the database context for Entity Framework Core.
    * **DTOs:** 
        * **Authentication:** Contains DTOs related to authentication (e.g., login, registration).
        * **Order:** Contains DTOs for order-related operations.
        * **Product:** Contains DTOs for product-related operations.
        * **User:** Contains DTOs for user-related operations.
    * **Handlers:** 
        * `AuthenticationHandler.cs`: Handles authentication logic.
        * `InviteCodeHandler.cs`: Handles operations related to invite codes.
        * `OrderHandlers.cs`: Handles order processing and management.
        * `ProductHandlers.cs`: Handles product management.
        * `RegistrationHandler.cs`:  Handles user registration.
        * `UserHandlers.cs`: Handles user management.
    * **Helpers:** 
        * `JsonValidator.cs`: Provides JSON validation utilities.
        * `JwtHelper.cs`:  Handles JWT generation and validation.
    * **https:** 
        * `server.pfx`:  Stores the HTTPS certificate.
    * **Models:** 
        * `InviteCode.cs`: Defines the `InviteCode` entity.
        * `Order.cs`: Defines the `Order` entity.
        * `Product.cs`: Defines the `Product` entity.
        * `User.cs`: Defines the `User` entity.
    * **Properties:** Stores application properties and settings.
    * **Repositories:** 
        * **Interfaces:** Contains interfaces for repositories to enable dependency injection and abstraction.
            * `IInviteCodeRepository.cs`: Interface for the `InviteCode` repository.
            * `IOrderRepository.cs`: Interface for the `Order` repository.
            * `IProductRepository.cs`: Interface for the `Product` repository.
            * `IUserRepository.cs`: Interface for the `User` repository.
        * `InviteCodeRepository.cs`: Implements the `IInviteCodeRepository`.
        * `OrderRepository.cs`: Implements the `IOrderRepository`.
        * `ProductRepository.cs`: Implements the `IProductRepository`.
        * `UserRepository.cs`: Implements the `IUserRepository`.

## Setup

Please refer to the [documentation](https://github.com/DanHumm/DummySupplier/tree/main/Documentation) directory for setup instructions and you can pull the latest package featured in the repo for a fully functioning representation of this project.

## Feedback

This was my first project creating a RESTful API using ASP.NET Core. Please feel free to leave any feedback which will help me improve and allow me to write cleaner, scalable and maintainable code. 
