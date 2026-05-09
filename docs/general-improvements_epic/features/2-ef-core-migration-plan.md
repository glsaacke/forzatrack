# EF Core Migration Implementation Plan

Replace all raw ADO.NET repositories with EF Core backed by the existing MySQL schema. The existing table structure and column names are preserved. All four entities (User, Car, Record, Build) are migrated at once.

---

## Tasks

### 1. Swap the EF Core provider package
- The project already has `Microsoft.EntityFrameworkCore` and `Microsoft.EntityFrameworkCore.SqlServer` installed, but the database is MySQL.
- Remove `Microsoft.EntityFrameworkCore.SqlServer` from `api/api.csproj`.
- Add `Pomelo.EntityFrameworkCore.MySql` (the community MySQL provider, which supports EF Core 9).

### 2. Create `AppDbContext`
**File:** `api/core/data/AppDbContext.cs` (new file)
- Inherit from `DbContext`.
- Declare `DbSet<User>`, `DbSet<Car>`, `DbSet<Record>`, `DbSet<Build>`.
- Override `OnModelCreating` to map the existing `snake_case` column and table names to PascalCase C# properties using Fluent API (since the schema must not change):

```csharp
modelBuilder.Entity<User>(e => {
    e.ToTable("Users");
    e.HasKey(u => u.UserId);
    e.Property(u => u.UserId).HasColumnName("user_id");
    e.Property(u => u.Username).HasColumnName("username");
    e.Property(u => u.Email).HasColumnName("email");
    e.Property(u => u.Password).HasColumnName("password");
    e.Property(u => u.Deleted).HasColumnName("deleted");
});
// Repeat for Car, Record, Build
```

- Use EF Core's global query filter on each entity to automatically exclude soft-deleted rows:
  ```csharp
  e.HasQueryFilter(u => u.Deleted == 0);
  ```
  This replaces the manual `deleted = 0` clauses scattered across the raw SQL queries.

### 3. Register `AppDbContext` in `Program.cs`
- Replace the `connectionString` raw usage with:
  ```csharp
  builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
  ```
- Remove the per-repository `AddScoped<IXRepository, XRepository>(provider => new XRepository(connectionString))` registrations and replace them with standard `AddScoped<IXRepository, XRepository>()` (the DbContext will be injected automatically).

### 4. Rewrite `UserRepository`
**File:** `api/core/services/UserService/UserRepository.cs`
- Replace the `string cs` constructor parameter with `AppDbContext context`.
- Rewrite each method using LINQ / EF Core:
  - `GetAllUsers` → `context.Users.ToList()`
  - `GetUserByID` → `context.Users.Find(id)` or `FirstOrDefault`
  - `GetUserByEmail` → `context.Users.FirstOrDefault(u => u.Email == email)`
  - `CreateUser` → `context.Users.Add(user); context.SaveChanges()`
  - `UpdateUser` → fetch entity, update fields, `context.SaveChanges()`
  - `SetUserDeleted` → fetch entity, set `Deleted = 1`, `context.SaveChanges()`
  - `DeleteUser` → `context.Users.Remove(entity); context.SaveChanges()`

### 5. Rewrite `CarRepository`
**File:** `api/core/services/CarService/CarRepository.cs`
- Same pattern as UserRepository. Constructor takes `AppDbContext`.

### 6. Rewrite `RecordRepository`
**File:** `api/core/services/RecordService/RecordRepository.cs`
- Same pattern. Note: the `AddDate` column should be handled with `DateTime.UtcNow` on insert.

### 7. Rewrite `BuildRepository`
**File:** `api/core/services/BuildService/BuildRepository.cs`
- Same pattern.

### 8. Remove `Connection.cs`
**File:** `api/core/data/Connection.cs`
- This file currently holds the unused hardcoded connection string helper. Delete it once the migration is complete.

### 9. Verify no raw SQL references remain
- Search for `MySqlConnection`, `MySqlCommand`, `DataTable`, and `DataRow` usages across the codebase and confirm they have all been removed.

### 10. Smoke test all endpoints
- Run the API locally and exercise each entity's CRUD endpoints via Swagger or `api.http` to confirm behaviour is identical to the old implementation.

---

## Notes
- **Do not run `dotnet ef migrations add`** unless the schema actually needs changing — EF Core can target an existing database without migration history using `EnsureCreated` or by simply pointing at the existing DB. The schema is owned by the DB, not by EF migrations, for now.
- The global `HasQueryFilter` for `deleted = 0` means hard-delete operations will need `context.Users.IgnoreQueryFilters().Find(id)` to locate the soft-deleted row first, or the hard-delete method should call `Remove` directly using the primary key.
- `Pomelo.EntityFrameworkCore.MySql` requires the server version to be detected or specified; `ServerVersion.AutoDetect(connectionString)` handles this automatically on startup.
- `CreateUser` is currently a `void` method but could be updated to return the inserted entity (using `context.SaveChanges()` + reading back the generated ID) — this is already how the service layer works by calling `GetUserByEmail` after insertion. EF Core will populate the ID automatically after `SaveChanges`.
