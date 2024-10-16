# cheetah-learning

## Setup
1. Make sure your machine has .net SDK (7.0.100) if not please download it form <a href="https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.110-windows-x64-installer"><h1>here<h1></a> 
2. Once code cloned. Please change the connection string in appsettings.json. Replace server name with your local MSSQL server name.
3. Run the project and it will create the database automatically. We don't need to run the migrations manually.

## Clean Architecture Overview

## Technologies
1. Web API using<a href="https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-7.0">ASP.NET Core 7</a>
2. Data access with<a href="https://learn.microsoft.com/en-us/ef/core/">Entity Framework Core 7</a>
3. UI using<a href="https://mudblazor.com/getting-started/installation#prerequisites">MudBlazor 14</a>
4. CQRS with<a href="https://github.com/jbogard/MediatR">MediatR</a>
5. Object-Object Mapping with<a href="https://automapper.org/">AutoMapper</a>
6. Validation with<a href="https://fluentvalidation.net/">FluentValidation</a>
7. Testing with<a href="https://nunit.org/">NUnit</a>, <a href="https://fluentassertions.com/">FluentAssertions</a>, <a href="https://github.com/moq">Moq</a> & <a href="https://github.com/jbogard/Respawn">Respawn</a>

# Overview

## Domain
This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

## Application
This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

## Infrastructure
This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

# Database

## Migrations
The template uses Entity Framework Core and migrations can be run using the EF Core CLI Tools. Install the tools using the following command:
dotnet tool install --global dotnet-ef --version 7.0.0

Once installed, run the following command to execute migrations:

cd src\Infrastructure
dotnet ef migrations add "Initial" --startup-project ..\WebUI\Server

# Authentication
The authentication service uses JSON Web Tokens (JWT) for user authentication. When a user successfully logs in or registers, a JWT token is generated and returned, which can be used for subsequent authenticated requests.

## Endpoints
### 1. /account/login:
Method: POST
Input: JSON object containing email and password.
Output: ResultDto object containing authentication status and token.

### 2. /account/register:
Method: POST
Input: JSON object containing user details.
Output: ResultDto object containing registration status and token.

### 3. /account/name:
Method: GET
Output: User's username.
Configuration
The application's configuration can be found in the appsettings.json file. Here, you can customize settings such as JWT token expiration time and logging levels.

## JWT Token Generation:
When a user successfully logs in or registers, a JWT token is generated using the GenerateToken method from the IAccountService interface.
The GenerateToken method in the AccountService class creates a JWT token using the JwtSecurityTokenHandler and signs it with a symmetric key defined in the appsettings.json file.
The token contains claims such as the user's name, roles, and any custom claims.
The token is then returned to the client and can be included in subsequent requests as a means of authentication.

## Token Validation:
When a client makes a request to a protected endpoint, it includes the JWT token in the request header.
The server validates the token by checking its signature, issuer, audience, and expiration time using the JwtBearer authentication middleware.
If the token is valid, the request is processed; otherwise, an authentication error is returned.

# Authorization:

## Role-based Authorization:
The roles of a user are determined during authentication based on their role mappings stored in the database.
Roles are added to the JWT token as claims.
Protected endpoints can be decorated with authorization attributes such as **[Authorize(Roles = "Admin")]** to restrict access to users with specific roles.

## Endpoint Authorization:
Endpoints can be configured to require authentication using the [Authorize] attribute. This ensures that only authenticated users can access the endpoint.
Additionally, you can specify roles or policies using **[Authorize(Roles = "Admin")]** or **[Authorize(Policy = "RequireAdminRole")]** to further restrict access based on user roles or custom policies.

## Authorization Policies:
Authorization policies can be defined in the _program.cs file_ using the _builder.Services.AddAuthorization()_ method.
Policies can specify requirements such as role membership, custom claims, or other conditions for access to resources.
These policies can then be applied to controllers or individual endpoints using the **[Authorize(Policy = "MyPolicy")]** attribute.