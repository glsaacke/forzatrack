# General Improvements Scope

## Proposed Features

### Password Hashing
- Hash all passwords at rest using BCrypt. Run a one-time migration script to hash the existing plain-text passwords in the database before the updated API goes live. Move login credential transmission from a GET query string to a POST request body.
Status:

### JWT Authentication
- Replace the current `sessionStorage` user ID pattern with short-lived JWTs. On successful login the API issues a signed token; the frontend stores it and sends it as a Bearer token on subsequent requests. The existing API key middleware remains in place as a secondary layer.
Status:

### EF Core Migration
- Replace all raw ADO.NET repositories with EF Core backed by the existing MySQL schema. No schema changes beyond what is required to support the new auth features (e.g., a token-related column if needed and approved).
Status:

### Input Validation
- Add server-side validation attributes to all request DTOs (required fields, max lengths, valid email format). The API should return a structured 400 response for invalid input rather than passing bad data to the database.
Status:

### Rate Limiting on Auth Endpoints
- Apply per-IP request rate limits to the login and signup endpoints to protect against brute-force and credential-stuffing attacks.
Status:

### Middleware Cleanup
- Remove the redundant custom `DotEnv.cs` middleware now that the `DotNetEnv` NuGet package is in use. Review the API key middleware for any issues and ensure it continues to function correctly alongside the new JWT auth layer.
Status:
