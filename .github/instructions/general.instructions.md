---
applyTo: "**"
---

# ForzaTrack — General Agent Instructions

ForzaTrack is a lap-time tracking app for Forza Horizon. Users log race records tied to their cars and builds. The workspace contains two independent projects:

- **`api/`** — ASP.NET Core 8 REST API (C#), deployed to Fly.io
- **`react-app/`** — React 18 SPA (Vite), deployed to Vercel

Both are containerized and can be run together via `docker-compose.yaml`.

---

## Architecture Overview

```
forzatrack/
├── api/                        # Backend — see api.instructions.md
│   └── core/
│       ├── controllers/        # HTTP endpoints + request DTOs
│       ├── models/             # Domain models + response DTOs
│       ├── services/           # Service + Repository interfaces/implementations per entity
│       ├── data/               # DB connection helper (unused — connection injected via env)
│       └── middleware/         # API key auth, .env loading
├── react-app/                  # Frontend — see react-app.instructions.md
│   └── src/
│       ├── pages/              # Full-page route components
│       ├── components/         # Reusable UI components
│       ├── services/           # API call functions (api.js)
│       └── styles/             # Per-component CSS files
└── docker-compose.yaml         # Local dev orchestration
```

---

## Domain Model

The core entities and their relationships:

- **User** — An account. Identified by `user_id`. Has `username`, `email`, `password`.
- **Car** — A vehicle in the game. Identified by `car_id`. Has `make`, `model`, `year`.
- **Record** — A lap time entry. Belongs to a User and a Car. Has `event`, `class_rank`, `time_min/sec/ms`, `cpu_diff`, `date`.
- **Build** — A car setup/tuning configuration. Belongs to a User and a Car. Has performance stats (`speed_st`, `handling_st`, `acceleration_st`, etc.) and `rank`.

All entities use a **soft delete** pattern: a `deleted` integer column (0 = active, 1 = deleted). Queries always filter by `deleted = 0`. Hard delete endpoints exist but are not used by the frontend.

---

## Database

- **Provider**: MySQL on AWS RDS (`us-east-1`)
- **Access pattern**: Raw ADO.NET — `MySqlConnection` + `MySqlCommand` + `DataTable`. No active ORM usage despite EF Core being installed.
- **Connection string**: Provided via `DATABASE_CONNECTION` environment variable.
- **Conventions**: DB column names are `snake_case`; C# model properties are `PascalCase`.
- All queries must use **parameterized queries** (`@param` syntax) to prevent SQL injection.

---

## Authentication & Security

- **API Key**: All API requests (except `/swagger` and `/api/health`) require an `X-Api-Key` header. The key is set via the `API_KEY` environment variable and validated in `ApiKeyMiddleware`.
- **User Auth**: Email + password. The `AuthenticateUser` endpoint returns a `UserDto` (no password field). The frontend stores `userId` in `sessionStorage`.
- **No JWT**: There are no bearer tokens. Route protection on the frontend is a simple `sessionStorage` check in `PrivateRoute`.
- **CORS**: Configured to allow `localhost:5173`, `localhost:5174`, `forzatrack.vercel.app`, and Vercel preview deployment URLs.

---

## Environment Variables

### API
| Variable | Description |
|---|---|
| `DATABASE_CONNECTION` | Full MySQL connection string |
| `API_KEY` | Secret used to validate `X-Api-Key` header |

### React App
| Variable | Description |
|---|---|
| `VITE_API_URL` | Base URL of the API (e.g. `http://localhost:8080`) |
| `VITE_API_KEY` | API key sent with every request |

Both are loaded from a `.env` file at project root (API uses `DotNetEnv`; React uses Vite's built-in `.env` loading).

---

## Local Development

```bash
# Run both projects together
docker compose up

# Or run individually:
cd api && dotnet run          # API on :8080
cd react-app && npm run dev   # Frontend on :5173
```

---

## Deployment

- **API**: Fly.io (`api/fly.toml`). Region: `iad` (Atlanta). Exposed on port 8080. Deployed via `fly deploy`.
- **Frontend**: Vercel (`react-app/vercel.json`). SPA routing configured with a catch-all rewrite to `index.html`.

---

## Adding a New Entity (Full-Stack Pattern)

Follow the existing pattern exactly. Replace `Entity` with your entity name.

### 1. Backend

**a. Domain model** → `api/core/models/Entity.cs`
```csharp
public class Entity {
    public int? EntityId { get; set; }
    public int UserId { get; set; }
    // ... fields
    public int Deleted { get; set; }
}
```

**b. Request DTO** → `api/core/controllers/models/EntityRequest.cs`
```csharp
public class EntityRequest {
    public int UserId { get; set; }
    // ... input fields (no EntityId, no Deleted)
}
```

**c. Response types** (if needed) → `api/core/models/responses/`

**d. Repository interface** → `api/core/services/EntityService/IEntityRepository.cs`
```csharp
public interface IEntityRepository {
    Task<List<Entity>> GetAllEntities();
    Task<Entity> GetEntityById(int id);
    Task<DefaultResponse> CreateEntity(Entity entity);
    Task<DefaultResponse> UpdateEntity(int id, Entity entity);
    Task<DefaultResponse> SetEntityDeleted(int id);
    Task<DefaultResponse> DeleteEntity(int id);
}
```

**e. Service interface** → `api/core/services/EntityService/IEntityService.cs`  
Mirrors the repository interface.

**f. Repository implementation** → `api/core/services/EntityService/EntityRepository.cs`  
- Constructor receives `string connectionString`
- Each method opens a `MySqlConnection`, executes parameterized `MySqlCommand`s
- Returns typed results (use `DataTable` → model mapping for reads)

**g. Service implementation** → `api/core/services/EntityService/EntityService.cs`  
- Constructor receives `IEntityRepository`
- Delegates all calls to the repository

**h. Controller** → `api/core/controllers/EntityController.cs`
```csharp
[Route("api/[controller]")]
[ApiController]
public class EntityController : ControllerBase {
    private readonly IEntityService _entityService;
    public EntityController(IEntityService entityService) { _entityService = entityService; }
    // Use action names: GetAllEntities, GetEntityById, CreateEntity, UpdateEntity, SetEntityDeleted, DeleteEntity
}
```

**i. Register in DI** → `api/Program.cs`
```csharp
builder.Services.AddScoped<IEntityService, EntityService>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>(provider =>
    new EntityRepository(connectionString));
```

### 2. Frontend

**a. API functions** → `react-app/src/services/api.js`  
Follow existing patterns: `getEntitiesByUserId`, `createEntity`, `setEntityDeleted`, etc.

**b. Page component** → `react-app/src/pages/EntityPage.jsx`  
Manage local state with `useState`. Fetch data with `useEffect` on mount.

**c. Register route** → `react-app/src/App.jsx`  
Add route under `/dashboard/entity` wrapped in `<PrivateRoute>`.

**d. Add CSS** → `react-app/src/styles/EntityPage.css`  
Import at the top of the page component.

---

## What Not to Do

- Do not add Redux, Context API, or any global state library unless explicitly requested.
- Do not switch from raw ADO.NET to EF Core or Dapper without instruction.
- Do not use hard deletes on entities — always prefer `SetEntityDeleted`.
- Do not add password hashing or JWT without scoping the change to what was requested.
- Do not add new npm packages or NuGet packages without confirming with the user.

## Additional Instructions
- Ask the user questions before making assumptions about requirements or implementation details.
- Always follow the existing code style and architectural patterns unless explicitly instructed to change them.
- Update instruction files as needed when new patterns or conventions are established in the codebase.
