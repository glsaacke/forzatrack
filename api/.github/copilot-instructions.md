# ForzaTrack Backend - GitHub Copilot Instructions

## Project Overview
This is a .NET 8.0 Web API backend for ForzaTrack, a racing game statistics tracking application. The API follows a layered architecture with controllers, services, and repositories.

## Technology Stack
- **Framework**: ASP.NET Core 8.0
- **Database**: MySQL
- **ORM**: Raw ADO.NET with MySql.Data
- **Authentication**: API Key-based authentication via custom middleware
- **Environment Management**: DotNetEnv for .env file loading
- **Documentation**: Swagger/OpenAPI

## Architecture Patterns

### Layered Architecture
The application follows a three-tier architecture:
1. **Controllers** (`core/controllers/`) - Handle HTTP requests and responses
2. **Services** (`core/services/`) - Business logic layer
3. **Repositories** (`core/services/[Entity]Service/`) - Data access layer

### Folder Structure
```
api/
├── core/
│   ├── controllers/          # API endpoint controllers
│   │   └── models/          # Request DTOs
│   ├── services/            # Business logic services
│   │   ├── [Entity]Service/
│   │   │   ├── I[Entity]Service.cs
│   │   │   ├── [Entity]Service.cs
│   │   │   ├── I[Entity]Repository.cs
│   │   │   └── [Entity]Repository.cs
│   ├── models/              # Domain models and DTOs
│   │   └── responses/       # Response DTOs
│   ├── middleware/          # Custom middleware
│   └── data/                # Database connection utilities
├── Program.cs               # Application entry point
└── appsettings.json         # Configuration
```

## Coding Standards

### Naming Conventions
- **Controllers**: `[Entity]Controller` (e.g., `UserController`)
- **Services**: `I[Entity]Service` (interface), `[Entity]Service` (implementation)
- **Repositories**: `I[Entity]Repository` (interface), `[Entity]Repository` (implementation)
- **Models**: PascalCase for classes, properties
- **Variables**: camelCase for local variables, private fields
- **HTTP Route**: `api/[controller]`

### Controller Patterns
```csharp
[Route("api/[controller]")]
[ApiController]
public class EntityController : ControllerBase
{
    private IEntityService entityService;
    private ILogger<EntityController> logger;
    
    public EntityController(IEntityService entityService, ILogger<EntityController> logger)
    {
        this.entityService = entityService;
        this.logger = logger;
    }
    
    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        try
        {
            var entities = entityService.GetAll();
            
            if (entities == null || entities.Count == 0)
            {
                logger.LogWarning("No entities found.");
                return NotFound(new { Message = "No entities found." });
            }
            
            return Ok(entities);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching entities.");
            throw;
        }
    }
}
```

### Service Pattern
```csharp
public class EntityService : IEntityService
{
    private IEntityRepository entityRepository;
    
    public EntityService(IEntityRepository entityRepository)
    {
        this.entityRepository = entityRepository;
    }
    
    public List<Entity> GetAll()
    {
        var entities = entityRepository.GetAll();
        return entities;
    }
}
```

### Repository Pattern
```csharp
public class EntityRepository : IEntityRepository
{
    private readonly string cs;
    
    public EntityRepository(string cs)
    {
        this.cs = cs;
    }
    
    public List<Entity> GetAll()
    {
        List<Entity> entities = new List<Entity>();
        
        using var con = new MySqlConnection(cs);
        con.Open();
        
        string stm = "SELECT * FROM Entities";
        using var cmd = new MySqlCommand(stm, con);
        using var reader = cmd.ExecuteReader();
        
        DataTable dataTable = new DataTable();
        dataTable.Load(reader);
        
        foreach (DataRow row in dataTable.Rows)
        {
            Entity entity = new Entity
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString()
            };
            entities.Add(entity);
        }
        
        return entities;
    }
}
```

## Dependency Injection

### Service Registration Pattern
All services and repositories are registered in `Program.cs` with scoped lifetime:

```csharp
builder.Services.AddScoped<IEntityService, EntityService>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>(provider =>
{
    return new EntityRepository(connectionString);
});
```

## CORS Configuration
The API supports specific origins for CORS:
- Development: `http://localhost:5173`
- Production: `https://forzatrack.vercel.app`

Always use:
- `AllowAnyHeader()`
- `AllowAnyMethod()`
- `WithHeaders("X-Api-Key")`
- `AllowCredentials()`

## Security

### API Key Middleware
All endpoints (except Swagger) require an API key in the `X-Api-Key` header. This is enforced by `ApiKeyMiddleware`.

### Password Storage
⚠️ **Note**: Passwords are currently stored in plain text. This should be updated to use hashing (BCrypt, PBKDF2, or Argon2) in production.

## Error Handling
- Use try-catch blocks in controllers
- Log warnings with `logger.LogWarning()` for expected issues (e.g., not found)
- Log errors with `logger.LogError(ex, message)` for exceptions
- Return appropriate HTTP status codes:
  - `200 OK` - Success with data
  - `404 NotFound` - Resource not found
  - `400 BadRequest` - Invalid request
  - `401 Unauthorized` - Missing/invalid API key

## Database Practices
- Use parameterized queries with `@parameter` syntax
- Always call `cmd.Prepare()` before `ExecuteNonQuery()` for INSERT/UPDATE
- Use `DataTable.Load(reader)` pattern for reading results
- Open and dispose connections properly with `using` statements
- Pass connection string via constructor dependency injection

## Request/Response Patterns

### Request Models
Place in `core/controllers/models/`:
```csharp
public class EntityRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}
```

### Response Models
Place in `core/models/responses/`:
```csharp
public class AuthResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public UserDto User { get; set; }
}
```

### Domain Models
Place in `core/models/`:
```csharp
public class Entity
{
    public int EntityId { get; set; }
    public string Name { get; set; }
}
```

## Environment Variables
- Use DotNetEnv for loading `.env` files
- Load in `Program.cs` before service configuration
- Access via `Environment.GetEnvironmentVariable("KEY")`
- Required variables:
  - `DATABASE_CONNECTION` - MySQL connection string
  - `API_KEY` - API authentication key

## Swagger Configuration
- Include API Key security definition
- Require API Key for all endpoints via security requirement
- Swagger UI available in development mode only

## Best Practices
1. **Always inject ILogger** into controllers for diagnostics
2. **Use interfaces** for all services and repositories
3. **Keep controllers thin** - delegate business logic to services
4. **Validate input** - check for null request bodies
5. **Return consistent responses** - use standard response models
6. **Follow REST conventions** - use appropriate HTTP verbs and status codes
7. **Database connections** - always use `using` statements for proper disposal
8. **SQL injection prevention** - always use parameterized queries
9. **Logging** - log warnings for expected failures, errors for exceptions
10. **Separation of concerns** - controllers handle HTTP, services handle logic, repositories handle data

## Common Entities
- **User**: UserId, Username, Email, Password, Deleted
- **Car**: CarId, Name, Year, Class, etc.
- **Build**: BuildId, BuildName, UserId, CarId, etc.
- **Record**: RecordId, UserId, BuildId, Track, Time, etc.

## Testing
When creating endpoints:
1. Test with Swagger UI in development
2. Use `api.http` file for HTTP requests
3. Ensure API key is included in headers
4. Verify CORS headers for frontend integration
