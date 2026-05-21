# Middleware Cleanup Implementation Plan

Remove the redundant custom `DotEnv.cs` class now that `DotNetEnv` (the NuGet package) is already handling `.env` loading. Review `ApiKeyMiddleware` for correctness alongside the new JWT authentication layer added in this epic.

---

## Tasks

### 1. Remove the custom `DotEnv` class
**File:** `api/core/middleware/DotEnv.cs`
- Delete this file. The `DotNetEnv` NuGet package (`Env.Load()` call in `Program.cs`) already handles `.env` loading.

### 2. Remove the dead `DotEnv.Load` call from `Program.cs`
**File:** `api/Program.cs`
- Remove the lines that instantiate the custom loader:
  ```csharp
  // Remove these:
  var root = Directory.GetCurrentDirectory();
  var dotenv = Path.Combine(root, ".env");
  DotEnv.Load(dotenv);
  ```
- Keep the `Env.Load()` call from `DotNetEnv` — this is the one that should remain.
- Remove the `using api.core.middleware;` import if it is no longer referenced after `DotEnv.cs` is gone (check that `ApiKeyMiddleware` is still referenced through its own using).

### 3. Review `ApiKeyMiddleware` bypass list
**File:** `api/core/middleware/ApiKeyMiddleware.cs`
- The middleware currently bypasses only `/swagger` and `/api/health`.
- With JWT auth added, the login endpoint (`/api/User/AuthenticateUser`) and signup endpoint (`/api/User/CreateUser`) must also be reachable without an API key from any client — or they must be included in the exemption list **if** the decision is that unauthenticated public clients (e.g., a future mobile app) should be able to call them without possessing the key.
- If the API key is considered a client-app identifier (not a user secret), these endpoints can stay gated behind it as they currently are. Confirm the desired behaviour and update the exemption list accordingly.

### 4. Fix case inconsistency in `ApiKeyMiddleware`
**File:** `api/core/middleware/ApiKeyMiddleware.cs`
- The middleware defines the header constant as `"X-API-Key"` but the Swagger definition in `Program.cs` uses `"X-Api-Key"` (different casing). HTTP headers are case-insensitive, so this is not a bug, but standardising to one casing (`X-Api-Key`) improves readability.

### 5. Remove the debug `Console.WriteLine` in `ApiKeyMiddleware`
**File:** `api/core/middleware/ApiKeyMiddleware.cs`
- Line `System.Console.WriteLine(_apiKey);` logs the raw API key to stdout on every rejected request. This is a security issue — remove it.

### 6. Confirm `DotNetEnv` is loaded before `DATABASE_CONNECTION` is read
**File:** `api/Program.cs`
- After removing the custom `DotEnv.Load` call, verify that `Env.Load()` appears before `Environment.GetEnvironmentVariable("DATABASE_CONNECTION")` so the connection string is available at startup.

---

## Notes
- The `DotEnv.cs` file is located in `api/core/middleware/` but is not middleware in the ASP.NET Core sense — it is a static utility class. It can be deleted without touching the middleware pipeline.
- `Env.Load()` from `DotNetEnv` behaves the same way: it reads a `.env` file from the current working directory and sets environment variables. No behaviour change is expected.
