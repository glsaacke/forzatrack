# JWT Authentication Implementation Plan

Replace the current pattern of storing a raw `userId` in `sessionStorage` with short-lived JWTs. On successful login or signup the API issues a signed token. The frontend stores the token and sends it as a `Bearer` token on all subsequent requests. The existing API key middleware stays in place as a secondary, client-identity layer.

---

## Tasks

### 1. Add JWT NuGet packages
- Add `Microsoft.AspNetCore.Authentication.JwtBearer` to `api/api.csproj`.
- `System.IdentityModel.Tokens.Jwt` is pulled in transitively; no separate reference needed.

### 2. Add `JWT_SECRET` environment variable
- Add a `JWT_SECRET` key to the API's `.env` file (and to the Fly.io secrets).
- Read it in `Program.cs` alongside `DATABASE_CONNECTION` and `API_KEY`.

### 3. Register JWT authentication in `Program.cs`
- Call `builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(...)` with the signing key and standard validation parameters (issuer, audience, lifetime).
- Call `app.UseAuthentication()` and `app.UseAuthorization()` in the middleware pipeline, **after** `app.UseCors` and **before** `app.MapControllers`.

### 4. Create a `TokenService`
**File:** `api/core/services/TokenService.cs` (new file)
- A small helper that takes a `UserDto` and returns a signed JWT string.
- Token claims should include at minimum: `sub` (userId), `name` (username), `email`.
- Expiry: 24 hours is a reasonable starting point for a low-stakes app.

### 5. Update `AuthResponse` model
**File:** `api/core/models/responses/AuthResponse.cs`
- Add a `Token` property (`string?`) to carry the JWT back to the frontend on successful login/signup.

### 6. Update `UserService` to return a token
**File:** `api/core/services/UserService/UserService.cs`
- Inject `TokenService` into `UserService`.
- In `AuthenticateUser` and `CreateUser`, when authentication succeeds, call `TokenService.GenerateToken(userDto)` and set `AuthResponse.Token`.

### 7. Protect API controllers with `[Authorize]`
- Add `[Authorize]` to all controllers (or at action level) except the auth endpoints (`AuthenticateUser`, `CreateUser`) and the health check.
- Alternatively, add a global `AuthorizeFilter` in `Program.cs` and use `[AllowAnonymous]` on the exempted actions.
- This means the frontend must send `Authorization: Bearer <token>` on all data requests.

### 8. Update `ApiKeyMiddleware` (keep but scope correctly)
**File:** `api/core/middleware/ApiKeyMiddleware.cs`
- The API key check still runs, but now the auth/user endpoints also need to be exempted from it (they rely on JWT instead). Ensure the middleware exempt list covers these routes.

### 9. Update Swagger config for JWT
**File:** `api/Program.cs`
- Add a `Bearer` security definition to the Swagger `AddSwaggerGen` call so the dev UI can send JWTs for testing.

### 10. Update `api.js` — store and send JWT
**File:** `react-app/src/services/api.js`
- On login/signup success, store the token: `sessionStorage.setItem("token", response.token)`.
- Add a shared request helper (or update each fetch call) to include `Authorization: Bearer <token>` in the headers alongside `X-Api-Key`.

### 11. Update `PrivateRoute`
**File:** `react-app/src/components/PrivateRoute.jsx`
- Change the guard check from `sessionStorage.getItem("userId")` to `sessionStorage.getItem("token")`.
- Optionally, decode the JWT client-side (no library needed — base64 decode the payload) to extract `userId` and `username` where needed, replacing the current `sessionStorage.getItem("userId")` calls across pages.

### 12. Update logout
- On logout, clear both `userId` and `token` from sessionStorage (or just `token` once `userId` is removed from all pages).

---

## Notes
- Keep token expiry at 24 hours to start. Refresh tokens are out of scope for this epic.
- The `userId` stored in the JWT payload makes it the single source of truth — components should decode the token rather than keep a separate `userId` key in sessionStorage.
- No database table for tokens is needed since JWTs are stateless.
- The API key middleware and JWT auth are complementary: API key identifies the client app, JWT identifies the user.
