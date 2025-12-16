## Tech Stack

### Backend
- ASP.NET Core Web API (.NET 8)
- C#
- Entity Framework Core
- SQL Server (running in Docker)
- FluentValidation
- Global Exception Handling Middleware

### Frontend (planned)
- Angular
- TypeScript
- Tailwind CSS

### Dev & Tooling
- Gitpod (primary development environment)
- Docker & Docker Compose
- VS Code (with recommended extensions)
- Git Flow

## Repository Structure

The project is organized following **Clean Architecture**, separating responsibilities into clear layers.

- `AccessControlHub.Domain`  
  Pure business layer: entities, domain exceptions, repository abstractions.

- `AccessControlHub.Application`  
  Application logic: DTOs, services, validation rules (FluentValidation), use-case orchestration.

- `AccessControlHub.Infrastructure`  
  Data access: EF Core DbContext, repository implementations, migrations.

- `AccessControlHub.Api`  
  Presentation layer: controllers, middleware, API configuration.
  
- `docker-compose.yml` 
- `.gitpod.yml` 
- `.env.example` 
- `README.md` 

## Getting Started (Gitpod – Recommended)

Gitpod is the recommended environment to run this project because it provides a reproducible development setup without manual tool installation.

### Steps

1. Open the repository in **Gitpod**.
2. Gitpod will automatically install the required toolchain:
   - .NET 8 SDK
   - Node.js
   - Docker and Docker Compose
   
3. Create your local environment file:
   ```bash
   cp .env.example .env
   ```

4. Start SQL Server using Docker Compose:
    ```bash
    docker compose up -d
    ```

5. Create local application settings (these files are ignored by Git):
    ```bash
    cp AccessControlHub.Api/appsettings.Development.Local.example.json AccessControlHub.Api/appsettings.Development.Local.json
    ```

Update the database password so it matches the value defined in your .env file.

6. Restore dependencies and run the API:
    ```bash
    dotnet restore
    dotnet run --project AccessControlHub.Api
    ```

7. Open Swagger in your browser:
    ```bash
    http://localhost:<PORT>/swagger
    ```

## Local Development Setup (Without Gitpod)

This section describes how to run the project locally without using Gitpod.

### Prerequisites
- .NET SDK 8
- Docker
- Docker Compose
- (Optional) VS Code with the recommended extensions

### Steps

1. Clone the repository and switch to the development branch:
    ```bash
    git clone <repository-url>
    cd accesscontrol-hub
    git checkout develop
    ```
2. Create the environment file:
    ```bash
    cp .env.example .env
    ```

3. Start SQL Server using Docker Compose:
    ```bash
    docker compose up -d
    ```

4. Create local application settings:
    ```bash
    cp AccessControlHub.Api/appsettings.Development.Local.example.json AccessControlHub.Api/appsettings.Development.Local.json
    ```

5. Apply database migrations:
    ```bash
    dotnet ef database update -p AccessControlHub.Infrastructure -s AccessControlHub.Api
    ```
    
6. Run the API:
    ```bash
    dotnet run --project AccessControlHub.Api
    ```

## Database & Migrations

This project uses **Entity Framework Core** with SQL Server. Database schema changes are managed through migrations.

### Create a new migration
Use this command when you add or modify entities in the Domain/Infrastructure layer:

```bash
dotnet ef migrations add <MigrationName> -p AccessControlHub.Infrastructure -s AccessControlHub.Api -o Migrations
```

### Apply migrations to the database
Run this command to create or update the database schema:

```bash
dotnet ef database update -p AccessControlHub.Infrastructure -s AccessControlHub.Api
```

### Notes
- The database runs inside a Docker container.
- Make sure the container is running before applying migrations:
```bash
docker compose up -d
```

## Environment Configuration

This project uses environment-specific configuration files to keep sensitive information out of the repository.

### Ignored Files (Not Committed)

The following files contain secrets and must never be committed to Git:

- `.env`
- `AccessControlHub.Api/appsettings.Development.Local.json`

These files are included in `.gitignore`.

### Template Files (Committed)

The following template files are committed to the repository and should be used as a starting point for local configuration:

- `.env.example`
- `AccessControlHub.Api/appsettings.Development.Local.example.json`

### How to Configure Locally

1. Create the Docker environment file:
   ```bash
   cp .env.example .env
   ```

2. Create the local application settings file:
    ```bash
    cp AccessControlHub.Api/appsettings.Development.Local.example.json AccessControlHub.Api/appsettings.Development.Local.json
    ```

3. Update the values (such as database password) so they match between .env and the connection string.

This approach allows each developer to have their own local configuration without exposing sensitive data.

## Error Handling

The API implements a **global exception handling middleware** to ensure consistent and predictable error responses.

This middleware:
- Centralizes error handling logic
- Prevents unhandled exceptions from leaking implementation details
- Returns standardized JSON error responses

### Validation Error Example

```json
{
  "error": "ValidationError",
  "message": "Email already exists"
}
```

### Internal Server Error Example

```json
{
  "error": "InternalServerError",
  "message": "An unexpected error occurred"
}
```

### Notes

- In development, detailed error information is logged for debugging purposes.
- In production, only safe and generic error messages are returned to clients.
