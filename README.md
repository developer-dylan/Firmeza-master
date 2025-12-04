# Firmeza — Sales Management Solution

Firmeza is a full-stack sales management solution composed of three main projects: a REST API, an MVC admin web app and a Vue 3 SPA. The solution covers product and inventory management, sales, user management (with roles), reporting (Excel/PDF) and email notifications.

This README gathers architecture, setup, configuration, run and deployment instructions for local development and production-ready deployment.

---

## Repository layout

- `Firmeza.Api/` — ASP.NET Core API project (controllers, DTOs, AutoMapper, JWT authentication, MailKit email service).
- `Firmeza.Web/` — ASP.NET Core MVC admin site (DbContext, views, services for PDF/Excel, identity integration). Contains EF Core migrations and `SeedData`.
- `Firmeza.Client/` — Vue 3 SPA (Vite, Tailwind, Pinia) for public-facing catalog and cart.
- `Firmeza.Identity/` — Shared Identity model.
- `DOCS_SMTP_CONFIGURATION.md` — SMTP/email configuration notes.

## Technologies

- .NET 8 (C#) — ASP.NET Core Web API and MVC
- Entity Framework Core 8 — Npgsql provider for PostgreSQL (also references for SQLite and SQL Server)
- ASP.NET Core Identity — authentication/authorization
- JWT — API token authentication
- Vue 3 + Vite + Tailwind CSS + Pinia — frontend SPA
- EPPlus (Excel) and QuestPDF (PDF)
- AutoMapper, MailKit, Swashbuckle (Swagger)

## Prerequisites

- .NET SDK 8.x
- PostgreSQL (or another EF Core-compatible DB)
- Node.js (LTS >= 18) and npm
- (Optional) Docker & Docker Compose
- (Optional) `dotnet-ef` global tool for migrations: `dotnet tool install --global dotnet-ef`

## Configuration

Main configuration points (project-level `appsettings.json` or environment variables):

- `ConnectionStrings:DefaultConnection` — Database connection string.
- `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience` — JWT settings for `Firmeza.Api`.
- SMTP configuration (see `DOCS_SMTP_CONFIGURATION.md`).

Environment variable example (Linux / macOS / Windows PowerShell uses different syntax):

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=firmeza;Username=myuser;Password=mypassword"
export Jwt__Key="your-strong-secret"
export Jwt__Issuer="Firmeza"
export Jwt__Audience="FirmezaClient"
```

Or set them in each project's `appsettings.Development.json` for local development (do not commit secrets).

## Database: migrations & seeding

- Migrations are stored under `Firmeza.Web/Migrations` (the Web project owns the DbContext in this solution).
- Apply migrations:

```bash
dotnet ef database update --project Firmeza.Web
```

- Seeding: `Firmeza.Web` runs `SeedData.InitializeAsync` at startup to create roles (`Admin`, `Client`) and a default admin user if not present.

Default seeded admin credentials (development only):

- Email: `admin@gmail.com`
- Password: `admin123.`

Change or remove these credentials for production environments.

## Running locally (development)

1. Start PostgreSQL and ensure it is reachable.

2. Restore and build .NET projects:

```bash
dotnet restore
dotnet build
```

3. Apply migrations:

```bash
dotnet ef database update --project Firmeza.Web
```

4. Run the API (Kestrel configured for ports `7000` in `Program.cs`):

```bash
dotnet run --project Firmeza.Api/Firmeza.Api.csproj
```

5. (Optional) Run the MVC admin site:

```bash
dotnet run --project Firmeza.Web/Firmeza.Web.csproj
```

6. Run the SPA frontend:

```bash
cd Firmeza.Client
npm install
npm run dev
```

Vite dev server typically listens on `http://localhost:5173`.

## Common commands

- Build API and Web:

```bash
dotnet build Firmeza.Api
dotnet build Firmeza.Web
```

- Publish for production:

```bash
dotnet publish Firmeza.Api -c Release -o ./publish/api
dotnet publish Firmeza.Web -c Release -o ./publish/web
```

- Frontend build:

```bash
cd Firmeza.Client
npm run build
```

## Docker Compose (example)

Use this as a starting point for local containerized development. It brings up PostgreSQL and the API. Adjust as needed and do not store secrets in the compose file for production.

```yaml
version: "3.8"
services:
  postgres:
    image: postgres:15
    environment:
      POSTGRES_DB: firmeza
      POSTGRES_USER: firmeza_user
      POSTGRES_PASSWORD: firmeza_pass
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data

  api:
    build:
      context: ./Firmeza.Api
      dockerfile: Dockerfile
    environment:
      ConnectionStrings__DefaultConnection: Host=postgres;Port=5432;Database=firmeza;Username=firmeza_user;Password=firmeza_pass
      Jwt__Key: "replace-with-secure-key"
      Jwt__Issuer: "Firmeza"
      Jwt__Audience: "FirmezaClient"
    depends_on:
      - postgres
    ports:
      - "7000:7000"

volumes:
  pgdata:
```

Notes:

- Add a `web` service and a static file host for the SPA if you want a fully containerized local environment.
- On Linux you may prefer `network_mode: host` for easier host access to services.

## SMTP / Email

- Email configuration is required to send receipts and notifications. See `DOCS_SMTP_CONFIGURATION.md` for provider-specific instructions and sample settings (Gmail, SMTP host/port, TLS, app passwords).

## Security & production checklist

- Replace default seeded credentials.
- Secure secrets using environment variables, Key Vaults or a secrets manager.
- Use strong JWT signing keys and rotate keys periodically.
- Restrict CORS policies — do not use permissive policies in production.
- Enable HTTPS and configure certificates properly.

## Debugging & troubleshooting

- Use the Swagger UI exposed by `Firmeza.Api` in Development to test endpoints (`/swagger`).
- Check logs for EF Core or authentication errors.
- Common fixes: verify connection string, apply migrations, confirm SMTP credentials for emails.

## Contribution

- Fork, branch and open PRs against `main`.
- Add migrations when altering the data model and include migration files in PRs.
- Include tests for new features where applicable and follow the project's coding style.

## License

If a `LICENSE` file exists in the repository root, that file determines licensing. If not present, contact the project owner.

---

Next steps I can do for you (choose any):

- Create `docker-compose.yml` in the repo root (complete dev environment for `postgres`, `api`, `web` and optional `client`).
- Add `.env.example` files for each project documenting environment variables (no secrets).
- Add a GitHub Actions workflow to build `Api`, `Web`, and `Client` on push.

If you want one of those, tell me which and I will create it.

This README was generated and tailored to this repository.

# Firmeza Project

Firmeza is a sales management system built with a .NET 8 Web API backend and a Vue 3 frontend. It features user authentication, a product catalog, a shopping cart, and automated email notifications.

## Project Structure

- **Firmeza.Api**: Backend REST API (ASP.NET Core 8).
- **Firmeza.Client**: Frontend application (Vue 3 + Vite).
- **Firmeza.Web**: Legacy web application.

## Prerequisites

- .NET SDK 8.0 or later
- Node.js 18.0 or later
- PostgreSQL Database

## Setup and Configuration

### 1. Database

Ensure you have a PostgreSQL database running. Update the connection string in the API configuration file if necessary.

### 2. Email Configuration (SMTP)

To enable email receipts, you must configure SMTP credentials in `Firmeza.Api/appsettings.json`.

Example configuration structure:

```json
"Smtp": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "Username": "your-email@gmail.com",
  "Password": "your-app-password",
  "FromName": "Firmeza Store"
}
```

Note: For Gmail, use an App Password.

## Running the Application

### Backend (API)

1. Open a terminal in the `Firmeza.Api` directory.
2. Run the following command:
   ```bash
   dotnet run
   ```
3. The API will start on `http://localhost:7000`.

### Frontend (Client)

1. Open a terminal in the `Firmeza.Client` directory.
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   npm run dev
   ```
4. Access the application at `http://localhost:5173`.

## Key Features

- **Authentication**: Secure login and registration with JWT.
- **Catalog**: Product browsing with stock management.
- **Cart**: Slide-out shopping cart with real-time updates.
- **Profile**: User purchase history and account details.
- **Receipts**: Digital receipts available in the profile and sent via email.

## Troubleshooting

- **Sales not loading**: Ensure the API is running and the database connection is valid.
- **Emails not sending**: Verify the SMTP credentials in `appsettings.json`.
