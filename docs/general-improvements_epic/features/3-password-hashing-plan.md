# Password Hashing Implementation Plan

Hash all user passwords at rest using BCrypt. Run a one-time migration script to hash existing plain-text passwords. Move credential transmission off the current GET query string and onto a POST request body.

---

## Tasks

### 1. Add BCrypt NuGet package
- Add `BCrypt.Net-Next` to `api/api.csproj`.

### 2. Run migration script (before deploying new API)
- Write a standalone script (or a throwaway console app / seed method in `Program.cs`) that:
  1. Reads every row from the `Users` table.
  2. Hashes the plain-text `password` column with BCrypt.
  3. Writes the hash back to the same row.
- Run against the live database **before** deploying the updated API so no window exists where unhashed passwords are compared against hashes.

### 3. Update `UserService.AuthenticateUser`
**File:** `api/core/services/UserService/UserService.cs`
- Replace the plain-text comparison `user.Password == password` with `BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)`.

### 4. Update `UserRepository.CreateUser`
**File:** `api/core/services/UserService/UserRepository.cs`
- Before inserting a new user, hash the password:
  ```csharp
  user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
  ```

### 5. Move `AuthenticateUser` from GET to POST
**File:** `api/core/controllers/UserController.cs`
- Change `[HttpGet("AuthenticateUser")]` to `[HttpPost("AuthenticateUser")]`.
- Replace the `[FromQuery]` parameters `email` and `password` with a `[FromBody]` DTO (e.g., a new `LoginRequest` with `Email` and `Password` fields).

### 6. Add `LoginRequest` DTO
**File:** `api/core/controllers/models/LoginRequest.cs` (new file)
```csharp
public class LoginRequest {
    public string Email { get; set; }
    public string Password { get; set; }
}
```

### 7. Update `api.js` (frontend)
**File:** `react-app/src/services/api.js`
- Change `authenticateUser` from a GET fetch with query-string parameters to a POST fetch with a JSON body:
  ```js
  export async function authenticateUser(email, password) {
    const response = await fetch(`${BASE_URL}/User/AuthenticateUser`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'X-Api-Key': API_KEY },
      body: JSON.stringify({ email, password })
    });
    return response.json();
  }
  ```

### 8. Update `UpdateUser` password handling
**File:** `api/core/services/UserService/UserRepository.cs`
- If `UpdateUser` can change the password, hash the incoming value before writing it.

---

## Notes
- BCrypt's work factor should default to 12 (the `BCrypt.Net-Next` library default).
- The migration script must run **before** the new API binary is deployed — there is no mixed-mode fallback detection planned.
- The `password` column name in MySQL stays the same; only its stored value changes.
