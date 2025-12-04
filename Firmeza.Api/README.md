# Firmeza.Api

**Overview**

Firmeza.Api is the REST API component of the Firmeza solution. It exposes endpoints for authentication, clients, products and sales, and is intended to be consumed by web or mobile clients. The API integrates with ASP.NET Core Identity (backed by EF Core), issues JWT tokens for authentication, and includes Swagger for interactive documentation.

**Project structure**

- `Controllers`: API controllers (Auth, Clients, Products, Sales).
- `DTOs`: Data Transfer Objects for requests and responses.
- `Mappings`: AutoMapper profiles and mapping configuration.
- `Services`: API-specific services (for example `EmailService`).
- `obj`, `bin`: build artifacts.

**Technologies and tools**

- **Language / Platform**: C# / .NET 8 (TargetFramework `net8.0`).
- **ORM**: Entity Framework Core 8 with PostgreSQL (`Npgsql.EntityFrameworkCore.PostgreSQL`) and optional SQLite/SQL Server providers referenced.
- **Authentication**: ASP.NET Core Identity + JWT (`Microsoft.AspNetCore.Authentication.JwtBearer`).
- **AutoMapper**: `AutoMapper.Extensions.Microsoft.DependencyInjection` for DTO mapping.
- **Email**: `MailKit` for sending emails.
- **API docs**: `Swashbuckle.AspNetCore` (Swagger).
- **Development tools**: `dotnet` CLI, `dotnet-ef` for migrations.

**Prerequisites**

- .NET SDK 8.x installed.
- PostgreSQL instance (or other EF Core compatible DB) and a database created.
- (Optional) `dotnet-ef` tool: `dotnet tool install --global dotnet-ef`.
- (Optional) Docker for containerized runs.

**Important configuration**

- Connection string: set `DefaultConnection` in `appsettings.json` or as an environment variable `ConnectionStrings__DefaultConnection`.
- JWT settings: the API requires configuration under the `Jwt` section with at least:
  - `Jwt:Key` (symmetric signing key)
  - `Jwt:Issuer`
  - `Jwt:Audience`

  Example (development):

  {
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=5432;Database=firmeza;Username=myuser;Password=mypass"
    },
    "Jwt": {
      "Key": "your-very-strong-secret-key",
      "Issuer": "Firmeza",
      "Audience": "FirmezaClient"
    }
  }

- Kestrel ports are configured in `Program.cs` to listen on `localhost:5000` (HTTP) and `localhost:7000` (HTTPS) by default.

**Roles / Seed behavior**

On startup the API ensures the roles `Admin` and `Client` exist (see `Program.cs`). The admin user seed is implemented in the `Firmeza.Web` project (`Data/SeedData.cs`). If your deployment requires an initial admin user, run the Web app seed or create a user via the API and assign the `Admin` role.

**Migrations and database**

- The API references EF Core and can apply migrations against the configured database.
- To apply migrations from the `Firmeza.Api` project folder:

  - Install `dotnet-ef` if needed:
    - `dotnet tool install --global dotnet-ef`

  - Apply migrations:
    - `dotnet ef database update --project Firmeza.Api`

Note: Migrations may be shared with `Firmeza.Web` depending on how the DbContext is used; check the `Migrations` folder in the solution root or in `Firmeza.Web`.

**Common commands (local development)**

- Restore and build:
  - `dotnet restore`
  - `dotnet build Firmeza.Api.csproj`
- Run the API:
  - `dotnet run --project Firmeza.Api.csproj`
  - The Swagger UI is enabled in Development and is typically reachable at `https://localhost:7000/swagger` or `http://localhost:5000/swagger`.

**Run with Docker (basic example)**

- Build image (from `Firmeza.Api` folder):
  - `docker build -t firmeza-api .`
- Run container (pass connection string and JWT secrets via env vars):
  - `docker run -e "ConnectionStrings__DefaultConnection=Host=host.docker.internal;Port=5432;Database=firmeza;Username=myuser;Password=mypass" -e "Jwt__Key=yourkey" -e "Jwt__Issuer=Firmeza" -e "Jwt__Audience=FirmezaClient" -p 5000:5000 -p 7000:7000 firmeza-api`

Adjust networking options as needed for Linux (e.g. `--network host`) and avoid plaintext secrets for production.

**Swagger & CORS**

- Swagger is configured for interactive API exploration (Development only by default).
- A permissive CORS policy `AllowAll` is registered by default (allows any origin/method/header). Restrict this policy in production.

**Useful files / locations**

- `Program.cs` - application startup, JWT, Swagger, Kestrel ports and role creation.
- `Controllers/` - API endpoints.
- `DTOs/` - request/response DTOs.
- `Mappings/` - AutoMapper profiles.
- `Services/EmailService.cs` - example email sending service using MailKit.

**Deployment checklist**

- Secure connection strings and JWT secrets (use environment variables, secrets manager or vault).
- Review and restrict CORS policy for production.
- Ensure HTTPS and proper certificates are configured.

**Contribution & testing**

- When contributing, follow the existing structure for controllers and mappings, add migrations when needed with `dotnet ef migrations add <Name>` and write tests for new endpoints.

**License**

Check the repository root for a `LICENSE` file or consult the project owner.

---

If you want, I can also:

- Add a `docker-compose.yml` to lift PostgreSQL + this API for local development.
- Add a sample `appsettings.Development.json` template with recommended settings (without secrets).

This README was generated with assistance.
