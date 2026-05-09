# Input Validation Implementation Plan

Add server-side validation to all API request DTOs so that invalid or incomplete data is rejected at the controller boundary before reaching the service or database layer. The API returns a structured `400 Bad Request` for any validation failure.

---

## Tasks

### 1. Enable automatic model validation responses
- ASP.NET Core's `[ApiController]` attribute already returns a `ValidationProblemDetails` 400 response automatically when model state is invalid — no additional middleware is needed.
- Confirm that all controllers use `[ApiController]` (they currently do).

### 2. Annotate `UserRequest`
**File:** `api/core/controllers/models/UserRequest.cs`

| Field | Rules |
|---|---|
| `Username` | `[Required]`, `[StringLength(50, MinimumLength = 3)]` |
| `Email` | `[Required]`, `[EmailAddress]`, `[StringLength(254)]` |
| `Password` | `[Required]`, `[StringLength(100, MinimumLength = 8)]` |

### 3. Annotate `LoginRequest` (new DTO from the Password Hashing feature)
**File:** `api/core/controllers/models/LoginRequest.cs`

| Field | Rules |
|---|---|
| `Email` | `[Required]`, `[EmailAddress]` |
| `Password` | `[Required]` |

### 4. Annotate `RecordRequest`
**File:** `api/core/controllers/models/RecordRequest.cs`

| Field | Rules |
|---|---|
| `UserId` | `[Required]`, `[Range(1, int.MaxValue)]` |
| `CarId` | `[Required]`, `[Range(1, int.MaxValue)]` |
| `Event` | `[Required]`, `[StringLength(100)]` |
| `ClassRank` | `[Required]`, `[StringLength(10)]` |
| `TimeMin` | `[Range(0, 59)]` |
| `TimeSec` | `[Range(0, 59)]` |
| `TimeMs` | `[Range(0, 999)]` |
| `CpuDiff` | `[Required]`, `[StringLength(20)]` |

### 5. Annotate `BuildRequest`
**File:** `api/core/controllers/models/BuildRequest.cs`

| Field | Rules |
|---|---|
| `UserId` | `[Required]`, `[Range(1, int.MaxValue)]` |
| `CarId` | `[Required]`, `[Range(1, int.MaxValue)]` |
| `Rank` | `[Required]`, `[Range(1, 999)]` |
| `SpeedST` | `[Range(0.0, 10.0)]` |
| `HandlingST` | `[Range(0.0, 10.0)]` |
| `AccelerationST` | `[Range(0.0, 10.0)]` |
| `LaunchST` | `[Range(0.0, 10.0)]` |
| `BrakingST` | `[Range(0.0, 10.0)]` |
| `OffroadST` | `[Range(0.0, 10.0)]` |
| `TopSpeed` | `[Range(0.0, 500.0)]` |
| `ZeroToSixty` | `[Range(0.0, 30.0)]` |

### 6. Annotate `CarRequest`
**File:** `api/core/controllers/models/CarRequest.cs`
- Read the existing fields and add `[Required]` and `[StringLength]` as appropriate for `Make`, `Model`, and `[Range]` for `Year`.

### 7. Remove manual null checks in controllers
- Several controllers already have manual `if (request == null)` guards (e.g., `UserController.CreateUser`). These become redundant once `[ApiController]` automatic validation is in place. Remove them to avoid duplication, or leave them — either is fine since the attribute check runs first.

### 8. Verify error response shape in the frontend
**File:** `react-app/src/services/api.js` and affected page components
- ASP.NET Core's automatic 400 response body is `{ errors: { FieldName: ["message"] }, ... }`. Confirm existing frontend error handling won't break if it receives a 400 it didn't previously handle. Pages that only check `response.success` will simply see a falsy value.

---

## Notes
- `[Range]` values for game stats (speed, handling, etc.) are estimates based on Forza Horizon's in-game stat scales. Adjust if the actual range differs.
- The `Deleted` field on request DTOs should probably be removed entirely — callers should not be able to set `Deleted` on creation. The field defaults to `0` and is only changed via the dedicated `Set{Entity}Deleted` endpoints.
