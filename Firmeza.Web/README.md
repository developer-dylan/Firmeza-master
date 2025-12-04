# Firmeza.Web

**Overview**

Firmeza.Web is the ASP.NET Core (MVC) web application of the Firmeza project. It provides the administration UI, product and sales management, user management, and report generation (Excel/PDF). The app uses ASP.NET Core Identity for authentication and Entity Framework Core for data persistence.

**Project structure**

- `Controllers`: MVC controllers (Account, Products, Sales, Reports, etc.).
- `Data`: `AppDbContext`, EF Core migrations and `SeedData` used to initialize roles and the admin user.
- `DTOs`: Data Transfer Objects used by views and services.
- `Interfaces`: Service and repository contracts.
- `Repositories`: Data access implementations.
- `Services`: Business logic (PDF/Excel generation, products, sales, users).
- `Models`: Domain models and entities (includes `Entities/`).
- `ViewModels`: View-specific models (Create/Edit view models, etc.).
- `Views`: Razor views for the different application areas.
- `wwwroot`: Static assets (CSS, JS, and `recibos/` for generated receipts).
- `Migrations`: Entity Framework Core migrations.

**Technologies and tools**

- **Language / Platform**: C# / .NET 8 (TargetFramework `net8.0`).
- **ORM**: Entity Framework Core 8 (PostgreSQL provider via `Npgsql`; the project also references SQLite and SQL Server providers).
- **Default Database**: PostgreSQL (`Npgsql.EntityFrameworkCore.PostgreSQL`).
- **Authentication**: ASP.NET Core Identity with a referenced project `Firmeza.Identity`.
- **Excel generation**: `EPPlus`.
- **PDF generation**: `QuestPDF`.
- **Development tools**: `dotnet` CLI (SDK 8), `dotnet-ef` (for migrations).
- **Containerization (optional)**: `Dockerfile` is included (Linux base image target).

**Prerequisites**

- .NET SDK 8.x installed (see `Firmeza.Web.csproj` -> `TargetFramework` = `net8.0`).
- PostgreSQL (or another EF Core compatible database). Create a database and a user with proper permissions.
- (Optional) Docker to build and run containers.
- (Optional) `dotnet-ef` global tool: `dotnet tool install --global dotnet-ef`.
- (Optional) Node.js / npm if you work with the `Firmeza.Client` frontend (separate project).

**Important configuration**

- Connection string: set `DefaultConnection` in `appsettings.json` or via environment variables.
  - Example PostgreSQL connection string (replace values):
    - `Host=localhost;Port=5432;Database=firmeza;Username=myuser;Password=mypassword`
  - As environment variable: `ConnectionStrings__DefaultConnection`.
- `appsettings.Development.json` is available for local configuration overrides.
- SMTP settings and usage are documented in the repository root file `DOCS_SMTP_CONFIGURATION.md`.

**Default credentials (seed)**

On first run, `SeedData.InitializeAsync` seeds roles and an admin user (if not already present):

- Email: `admin@gmail.com`
- Password: `admin123.`

Change these credentials before deploying to production.

**Migrations and database**

- Migrations are stored under the `Migrations` folder.
- To apply migrations (from the `Firmeza.Web` project folder):

  - Install `dotnet-ef` if not already installed:

    - `dotnet tool install --global dotnet-ef`

  - Apply migrations to the database:
    - `dotnet ef database update --project Firmeza.Web` (or run `dotnet ef database update` from the `Firmeza.Web` folder).

**Common commands (local development)**

- Restore packages and build:
  - `dotnet restore`
  - `dotnet build Firmeza.Web.csproj`
- Run the application:
  - `dotnet run --project Firmeza.Web.csproj`
  - Or use hot reload:
    - `dotnet watch --project Firmeza.Web run`
- Publish (create deployable artifacts):
  - `dotnet publish Firmeza.Web.csproj -c Release -o ./publish`

**Run with Docker (basic example)**

- Build image (from `Firmeza.Web` folder):
  - `docker build -t firmeza-web .`
- Run container (pass connection string via environment variable):
  - `docker run -e "ConnectionStrings__DefaultConnection=Host=host.docker.internal;Port=5432;Database=firmeza;Username=myuser;Password=mypass" -p 5000:80 firmeza-web`

Note: Adjust `host.docker.internal` according to your environment (on Linux you may use `--network host` or the network address). Never expose credentials in plain text in production.

**Behavior / automatic actions**

- `Program.cs` configures `Npgsql` (PostgreSQL) as the default provider and calls `SeedData.InitializeAsync` at startup to ensure roles and the admin user exist.
- Identity tables are renamed in `OnModelCreating` (e.g. `users`, `roles`, `user_roles`, etc.).

**Useful locations**

- `Data/AppDbContext.cs` - DbContext configuration.
- `Data/SeedData.cs` - Role and admin seed logic.
- `Migrations/` - EF Core migrations.
- `Controllers/` - HTTP endpoints and controllers.
- `Services/` - Business logic (PDF/Excel, etc.).

**Deployment and production checklist**

- Before deploying:
  - Configure secure connection strings (secrets, environment variables or a vault).
  - Replace default seeded credentials and review password policies.
  - Set `ASPNETCORE_ENVIRONMENT=Production` and configure logging, HTTPS, and monitoring.

**Contribution and testing**

- When contributing, follow existing code style in controllers/services and add migrations with `dotnet ef migrations add <Name>`.

**License**

Check for a `LICENSE` file in the repository root (if present) or contact the project owner for licensing details.

---

If you want, I can also:

- Add a `docker-compose.yml` example that brings up PostgreSQL and the app for local development.
- Add CI/CD or deployment instructions for cloud platforms (Azure, DigitalOcean, etc.).

This README was generated with assistance.
