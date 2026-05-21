# Rate Limiting on Auth Endpoints Implementation Plan

Apply per-IP request rate limits to the login (`AuthenticateUser`) and signup (`CreateUser`) endpoints to protect against brute-force and credential-stuffing attacks.

---

## Tasks

### 1. Enable the built-in rate limiting middleware
- ASP.NET Core 7+ includes `Microsoft.AspNetCore.RateLimiting` in the framework — no additional NuGet package is required.

### 2. Define rate limit policies in `Program.cs`
**File:** `api/Program.cs`
- Add `builder.Services.AddRateLimiter(...)` before `builder.Build()`.
- Define a named policy (e.g., `"auth"`) using a fixed-window or sliding-window limiter:
  ```csharp
  builder.Services.AddRateLimiter(options => {
      options.AddFixedWindowLimiter("auth", opt => {
          opt.Window = TimeSpan.FromMinutes(1);
          opt.PermitLimit = 10;
          opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
          opt.QueueLimit = 0;
      });
      options.RejectionStatusCode = 429;
  });
  ```
- 10 requests per minute per IP is a reasonable starting limit for a low-traffic public app.

### 3. Register the middleware in the pipeline
**File:** `api/Program.cs`
- Call `app.UseRateLimiter()` after `app.UseCors()` and before `app.MapControllers()`.

### 4. Apply the policy to the auth actions
**File:** `api/core/controllers/UserController.cs`
- Add `[EnableRateLimiting("auth")]` to the `AuthenticateUser` and `CreateUser` action methods.
- All other endpoints remain unrestricted for now.

### 5. Partition by IP address (key selector)
- By default, ASP.NET Core's rate limiter is global. To make it per-IP, configure a `PartitionedRateLimiter` using `HttpContext.Connection.RemoteIpAddress` as the partition key:
  ```csharp
  options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
      RateLimitPartition.GetFixedWindowLimiter(
          context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
          _ => new FixedWindowRateLimiterOptions {
              Window = TimeSpan.FromMinutes(1),
              PermitLimit = 10,
          }));
  ```
  - Alternatively, apply the named policy only to the auth actions and rely on the partition at the global limiter level.

### 6. Handle 429 responses in the frontend
**File:** `react-app/src/pages/Login.jsx` and `react-app/src/pages/Signup.jsx`
- The API will return `429 Too Many Requests` when the limit is hit. The frontend should display a user-friendly message (e.g., "Too many attempts, please wait a moment and try again.") rather than a blank or unexpected error.

---

## Notes
- The limit of 10 per minute is a starting point. It can be tightened (e.g., 5 per minute) if brute-force attempts become an issue.
- Fly.io's load balancer preserves the client IP in `X-Forwarded-For`. If `RemoteIpAddress` returns the proxy IP instead of the client IP in production, configure `ForwardedHeadersOptions` in `Program.cs` to read from that header.
- Rate limiting only the auth endpoints keeps the implementation simple and avoids impacting normal user data operations.
