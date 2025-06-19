# Api-Web-Pulso

## Overview


## Features

- Layered architecture:
  - **Domain**: Entities and business models
  - **Application**: Interfaces, services, business logic
  - **Infrastructure**: Data persistence (EF Core DbContext), external services implementations
  - **ApiWebPulso**: Presentation layer with Controllers and Middleware
- Generic base controllers with reusable CRUD actions (List, Insert, Update, Delete)
- Middleware for automatic auditing of POST, PUT, DELETE requests
- Audit logs saved with user info, action, entity, timestamp, IP address, and user agent
- Dependency Injection and clean separation of concerns

## Technologies

- ASP.NET Core 7+
- Entity Framework Core
- SQL Server (or any EF Core compatible database)
- Swagger / OpenAPI for API documentation and testing
- C# 11

## Getting Started

### Prerequisites

- .NET 7 SDK or later
- SQL Server (or compatible DB)
- Visual Studio 2022 / VSCode or any C# IDE

### Auditing

The API automatically logs user actions on POST, PUT, and DELETE requests through a middleware component. Audit entries include:
- User ID (from authentication claims)
- HTTP method and endpoint
- Entity name and optional entity ID
- Requestor IP address
- User agent string
- Timestamp
- Logs are saved in the AuditLogs table in the database.

### Extending

- Create entities in the Domain layer
- Define repositories and services in Application layer
- Implement database context and repositories in Infrastructure
- Create concrete controllers inheriting from BaseController<T>
