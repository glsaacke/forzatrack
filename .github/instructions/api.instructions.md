---
applyTo: "api/**"
---

# ForzaTrack API — Agent Instructions

ASP.NET Core 8 REST API. C#. MySQL on AWS RDS via raw ADO.NET. Deployed to Fly.io.

---

## Project Entry Point

`api/Program.cs` is the single configuration file. It:
1. Loads `.env` via `DotNetEnv`
2. Reads `DATABASE_CONNECTION` and `API_KEY` from environment
3. Configures CORS (allowed origins hardcoded in the file)
4. Registers all services and repositories into the DI container
5. Adds `ApiKeyMiddleware` to the pipeline
6. Maps controllers
7. Enables Swagger in Development

**When adding a new entity, `Program.cs` always needs two new `AddScoped` registrations.**

---

## Layered Architecture

Every entity follows a strict 4-layer pattern:

```
Controller  →  IService  →  Service  →  IRepository  →  Repository (MySQL)
```

| Layer | Location | Responsibility |
|---|---|---|
| Controller | `core/controllers/EntityController.cs` | HTTP routing, parsing request DTO, calling service |
| Request DTO | `core/controllers/models/EntityRequest.cs` | Shape of the HTTP request body |
| Service Interface | `core/services/EntityService/IEntityService.cs` | Contract for business logic |
| Service Implementation | `core/services/EntityService/EntityService.cs` | Delegates to repository |
| Repository Interface | `core/services/EntityService/IEntityRepository.cs` | Contract for data access |
| Repository Implementation | `core/services/EntityService/EntityRepository.cs` | MySQL SQL execution |
| Domain Model | `core/models/Entity.cs` | Plain C# model matching DB schema |
| Response DTOs | `core/models/responses/` | Typed response shapes (`DefaultResponse`, `AuthResponse`) |

---

## Controller Conventions

```csharp
[Route("api/[controller]")]
[ApiController]
public class RecordController : ControllerBase
```

- Route base: `/api/[ControllerName]` (e.g., `/api/Record`)
- Action names follow the pattern `[Verb][Entity][Qualifier]`

### Standard Action Names

| Action | HTTP Method | Route | Notes |
|---|---|---|---|
| `GetAll{Entity}s` | GET | `/api/Entity/GetAll{Entity}s` | Returns all non-deleted |
| `Get{Entity}ById` | GET | `/api/Entity/Get{Entity}ById/{id}` | By primary key |
| `Get{Entity}sBy{Field}` | GET | `/api/Entity/Get{Entity}sBy{Field}?{field}={val}` | Query-string filter (e.g., `GetRecordsByUserId`) |
| `Create{Entity}` | POST | `/api/Entity/Create{Entity}` | Body is `EntityRequest` |
| `Update{Entity}` | PUT | `/api/Entity/Update{Entity}/{id}` | Body is `EntityRequest` |
| `Set{Entity}Deleted` | PUT | `/api/Entity/Set{Entity}Deleted/{id}` | Soft delete |
| `Delete{Entity}` | DELETE | `/api/Entity/Delete{Entity}/{id}` | Hard delete (rarely called) |

Special:
- `GET /api/health` — health check, exempt from API key middleware
- `GET /api/User/AuthenticateUser?email=&password=` — returns `AuthResponse`

---

## Domain Models

Located in `api/core/models/`. All nullable IDs (not set by caller), `Deleted` defaults to 0.

```csharp
// Example pattern — User.cs
public class User {
    public int? UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Deleted { get; set; }
}
```

- `UserDto` — strips `Password` from User for safe API responses
- `CreateUser` — used to carry data before insertion (same shape as `User`)
- `AuthResponse` — wraps `UserDto` + a `Success` bool and `Message` string
- `DefaultResponse` — wraps `Success` bool + `Message` string; returned by write operations

---

## Request DTOs

Located in `api/core/controllers/models/`. These are the shapes of request bodies.

```csharp
// RecordRequest.cs
public class RecordRequest {
    public int UserId { get; set; }
    public int CarId { get; set; }
    public string Event { get; set; }
    public string ClassRank { get; set; }
    public int TimeMin { get; set; }
    public int TimeSec { get; set; }
    public int TimeMs { get; set; }
    public string CpuDiff { get; set; }
    public DateTime Date { get; set; }
}
```

Never include `EntityId` or `Deleted` in request DTOs.

---

## Database Access Pattern

Repositories use raw ADO.NET — no ORM. Every repository:

1. Receives `string connectionString` in its constructor
2. Opens a `MySqlConnection` per method call (not shared)
3. Uses `MySqlCommand` with named `@param` placeholders
4. For reads: fills a `DataTable`, maps rows to model objects
5. Returns `DefaultResponse` for writes, typed models for reads

```csharp
// Read example
public async Task<List<Record>> GetRecordsByUserId(int userId) {
    using var connection = new MySqlConnection(_connectionString);
    await connection.OpenAsync();
    var command = new MySqlCommand(
        "SELECT * FROM Records WHERE user_id = @userId AND deleted = 0",
        connection);
    command.Parameters.AddWithValue("@userId", userId);
    var adapter = new MySqlDataAdapter(command);
    var table = new DataTable();
    adapter.Fill(table);
    return table.AsEnumerable().Select(row => new Record {
        RecordId = row.Field<int>("record_id"),
        UserId = row.Field<int>("user_id"),
        // ...
    }).ToList();
}

// Write example
public async Task<DefaultResponse> CreateRecord(Record record) {
    using var connection = new MySqlConnection(_connectionString);
    await connection.OpenAsync();
    var command = new MySqlCommand(
        "INSERT INTO Records (user_id, car_id, ...) VALUES (@userId, @carId, ...)",
        connection);
    command.Parameters.AddWithValue("@userId", record.UserId);
    // ...
    await command.ExecuteNonQueryAsync();
    return new DefaultResponse { Success = true, Message = "Record created." };
}
```

**Rules:**
- Always use `@param` placeholders — never string-concatenate SQL
- Always filter `WHERE deleted = 0` on read queries
- Soft delete sets `deleted = 1`, never removes the row
- Return `DefaultResponse { Success = false, Message = "..." }` on expected failures (e.g., duplicate key)

---

## Middleware

### `ApiKeyMiddleware`
- Validates `X-Api-Key` header against `API_KEY` env var
- Exempt paths: anything starting with `/swagger`, exact path `/api/health`
- Returns `401 Unauthorized` on mismatch
- Registered in `Program.cs` with `app.UseMiddleware<ApiKeyMiddleware>()`

### `DotEnv`
- Static helper that loads `.env` file from a given path
- Called early in `Program.cs` before any env reads

---

## Dependency Injection Registration (Program.cs)

For each new entity, add exactly:

```csharp
builder.Services.AddScoped<IEntityService, EntityService>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>(provider =>
    new EntityRepository(connectionString));
```

The `connectionString` variable is resolved earlier in `Program.cs` from `Environment.GetEnvironmentVariable("DATABASE_CONNECTION")`.

---

## Response Shapes

### `DefaultResponse`
```csharp
public class DefaultResponse {
    public bool Success { get; set; }
    public string Message { get; set; }
}
```
Used by all write operations (create, update, delete).

### `AuthResponse`
```csharp
public class AuthResponse {
    public bool Success { get; set; }
    public string Message { get; set; }
    public UserDto User { get; set; }
}
```
Used only by `AuthenticateUser` and `CreateUser`.

---

## CORS

Allowed origins defined in `Program.cs`:
- `http://localhost:5173`
- `http://localhost:5174`
- `https://forzatrack.vercel.app`
- Any `https://*.vercel.app` preview URL

When adding new deployment targets, add to the CORS policy in `Program.cs`.

---

## Deployment (Fly.io)

- Config: `api/fly.toml`
- Region: `iad`
- Internal port: `8080`
- Dockerfile: multi-stage `dotnet publish` → runtime image
- Deploy: `fly deploy` from the `api/` directory

---

## NuGet Packages

| Package | Version | Use |
|---|---|---|
| `MySql.Data` | 9.1.0 | MySQL connector (ADO.NET) |
| `DotNetEnv` | 3.1.1 | `.env` file loading |
| `Swashbuckle.AspNetCore` | 7.2.0 | Swagger/OpenAPI |
| `Microsoft.AspNetCore.Cors` | 2.3.0 | CORS middleware |
| `Microsoft.EntityFrameworkCore` | 9.0.2 | Installed but not actively used |

Do not add new packages without explicit instruction.
