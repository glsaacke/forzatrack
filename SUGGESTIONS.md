# ForzaTrack — Suggestions & Ideas

A code-review-driven list of improvements, new features, and architectural changes to make ForzaTrack more versatile, maintainable, and ready for future Forza game releases.

---

## Table of Contents

1. [Multi-Game & Versatility](#1-multi-game--versatility)
2. [Security Fixes](#2-security-fixes)
3. [New Features](#3-new-features)
4. [Backend Improvements](#4-backend-improvements)
5. [Frontend Improvements](#5-frontend-improvements)
6. [Data & Database](#6-data--database)
7. [Testing](#7-testing)
8. [DevOps & Infrastructure](#8-devops--infrastructure)
9. [Documentation](#9-documentation)

---

## 1. Multi-Game & Versatility

The biggest bottleneck to supporting new Forza titles (or any other racing game) is that game-specific data is baked directly into the React components and has no representation in the backend at all.

### 1.1 Add a `Game` entity to the data model
Right now there is no concept of a "game" anywhere in the codebase. Every record is implicitly tied to Forza Horizon 5. A minimal addition would be:

```
Games          (game_id, name, short_name, active)
EventTypes     (event_id, game_id, name)
ClassRanks     (rank_id, game_id, label, sort_order)
```

Each `Record` would then reference an `event_id` and `rank_id` foreign key instead of storing raw strings. This lets you add a new title (e.g., Forza Motorsport 2025) by inserting rows rather than shipping a code change.

### 1.2 Serve events, classes, and difficulty levels from the API
Currently the `AddRecordModal` component hard-codes every dropdown option:

```jsx
// react-app/src/components/AddRecordModal.jsx
["Goliath", "Colossus", "Gauntlet", "Titan", "Marathon", "Vulcan Sprint"]
["X", "S2", "S1", "A", "B", "C", "D", "E"]
["Unbeatable", "Pro", "Expert", …]
```

These should be fetched from the API (e.g., `GET /Game/{gameId}/Events`, `GET /Game/{gameId}/Classes`) so the UI always reflects whatever is in the database without a frontend re-deploy.

### 1.3 Add a game selector to the dashboard
A top-level game selector (or a per-record game field) would let users switch context between FH5, FM2025, and any future title, with all filters and analysis scoped to the selected game automatically.

### 1.4 Make car data game-aware
The `Cars` table currently stores any car globally. Different Forza titles have different rosters. Linking cars to games (or at minimum tagging records with a game) prevents cross-game data pollution.

### 1.5 Generalize the `ClassRank` concept
Different games use different classification systems (e.g., Motorsport uses D/C/B/A/S1/S2/X while Horizon adds "stock" or "off-road" classes). Storing class ranks in the DB rather than as an enum or hard-coded array would make the app work with any classification scheme.

---

## 2. Security Fixes

These are the most urgent items regardless of new features.

### 2.1 🔴 Hash passwords — do not store or compare plaintext
`UserService.cs` compares passwords in plaintext:
```csharp
if (user.Password == password && user.Deleted == 0)
```
Use **BCrypt.Net-Next** or ASP.NET Core's built-in `PasswordHasher<T>` to hash on creation and verify on login. This is the single most critical security issue in the project.

### 2.2 🔴 Remove commented-out database credentials from source
`api/core/data/Connection.cs` contains commented AWS RDS host, username, password, and database name. Even if rotated, these should never live in version-controlled source. Remove them immediately and rotate the credentials as a precaution.

### 2.3 🟡 Replace session-storage auth with JWT tokens
Storing a raw `userId` in `sessionStorage` provides no tamper protection. Issuing a signed JWT from the login endpoint (and validating it server-side on every request) is the standard approach and is straightforward with `Microsoft.AspNetCore.Authentication.JwtBearer`.

### 2.4 🟡 Validate event and class values on the backend
The `RecordController` / `RecordRepository` accept any string for `event` and `classRank`. Adding an allowlist check against the DB (or an enum) prevents garbage data and potential injection vectors.

### 2.5 🟡 Move the API base URL to an environment variable
`react-app/src/services/api.js` hard-codes `https://forzatrack.fly.dev/api`. It should read `import.meta.env.VITE_API_BASE_URL` so the same build artifact can target different environments.

### 2.6 🟡 Move CORS origins to configuration
`api/Program.cs` hard-codes `http://localhost:5173` and `https://forzatrack.vercel.app`. These should be read from an environment variable or `appsettings.json` so deployments don't require code changes.

---

## 3. New Features

### 3.1 Complete the Builds page
`react-app/src/pages/Builds.jsx` exists and has a placeholder, and the entire backend for builds is already implemented (`BuildController`, `BuildService`, `BuildRepository`). Wiring up the frontend to display, create, and delete builds is low-hanging fruit that delivers immediate value.

### 3.2 Edit records
Users can currently only delete a record — they cannot correct a typo in a time or change the car. Add an "Edit" action (pencil icon) that opens the existing `AddRecordModal` pre-populated with the record's values and calls a new `PUT /Record/UpdateRecord` endpoint.

### 3.3 Leaderboards / community records
Add a public leaderboard page (`/leaderboard`) that shows the top times per event + class across all users, without exposing private user data. This turns ForzaTrack from a personal journal into a competitive community tool.

### 3.4 Personal bests and trend charts
Show a "Personal Best" badge on the fastest time per event+class combo. Add a simple line chart (e.g., using Recharts) showing how a user's best time for a specific event has improved over multiple sessions.

### 3.5 Notes / tuning tips on records
Allow users to attach a free-text note to each record (e.g., "ran on Hard Tires, slight oversteer") — useful when reviewing what setup produced a fast lap.

### 3.6 Car search and autocomplete
`getAllCars()` returns every car in the database. As the list grows this becomes unwieldy. Replace the static dropdown in `AddRecordModal` with a type-ahead search input backed by `GET /Car/Search?q=ford`.

### 3.7 Car photo / screenshot upload
Let users attach an in-game screenshot to a record or build. Store images in an object store (e.g., Cloudflare R2, AWS S3) and save the URL in the database.

### 3.8 Record import / export (CSV)
Allow users to export their records as a CSV file for offline analysis in Excel/Google Sheets, and import from a CSV to migrate data or log offline sessions in bulk.

### 3.9 Date range filtering
Add a date range picker to the records dashboard so users can view only records from a specific season or tuning session.

### 3.10 "Best N records" / record count summary
Show a small stat bar above the table with totals such as "42 records across 6 events — Personal Best: 6:12.830 on Goliath (X class)."

---

## 4. Backend Improvements

### 4.1 Switch to Entity Framework Core with migrations
The backend already references EF Core packages but uses raw ADO.NET SQL via `MySql.Data`. Switching to EF Core with `Pomelo.EntityFrameworkCore.MySql` would:
- Provide type-safe LINQ queries instead of hand-written SQL strings
- Enable `dotnet ef migrations` for reproducible schema changes
- Eliminate the risk of typos in column names

### 4.2 Add structured error handling middleware
Controllers currently wrap everything in bare `try/catch` blocks that return inconsistent error shapes. A global exception-handling middleware (using `IExceptionHandler` in .NET 8) would normalize error responses and reduce boilerplate.

### 4.3 Add server-side pagination
`GET /Record/GetRecordsByUserId` returns all records for a user with no limit. Add `?page=1&pageSize=50` query parameters and a corresponding `totalCount` in the response envelope so the frontend can implement virtual scrolling or paging.

### 4.4 Add an API versioning strategy
As new games and features require breaking changes, versioning the API (`/api/v1/`, `/api/v2/`) or using header-based versioning (`api-version: 2.0`) prevents old clients from breaking.

### 4.5 Introduce proper logging
Replace implicit exception logging with structured logging via `ILogger<T>` in every service and repository. Add Serilog (or the built-in .NET logging providers) to ship logs to a sink (console for local, a log aggregator in production).

### 4.6 Add a health-check endpoint
Register ASP.NET Core's built-in health checks (`/healthz`) and wire it up to the Fly.io `[checks]` config in `fly.toml` so deployments validate liveness before cutting over traffic.

### 4.7 Rate limiting
Add ASP.NET Core's built-in rate-limiting middleware (available since .NET 7) to protect the authentication endpoints from brute-force attempts.

---

## 5. Frontend Improvements

### 5.1 Replace Bootstrap 4 with a modern component library or Tailwind CSS
Bootstrap 4 is end-of-life and the project mixes it with heavy custom CSS in ways that are hard to maintain. Migrating to **Tailwind CSS** (utility-first, pairs well with Vite) or **shadcn/ui** (accessible component library) would give a consistent design system.

### 5.2 Add proper loading and error states
Most `useEffect` data fetches have no loading spinner and no error boundary. Users see a blank table while data loads. Add skeleton loaders and a user-friendly error message when a fetch fails.

### 5.3 Move API calls to React Query (TanStack Query)
Currently every component manually manages `useState` + `useEffect` for async data. **TanStack Query** handles caching, background refetching, loading/error states, and optimistic updates out of the box, reducing boilerplate and bugs.

### 5.4 Improve the time input UX
The `AddRecordModal` uses a raw text field expecting `mm:ss.mmm` format. Replace it with three separate masked numeric inputs (minutes / seconds / milliseconds) with inline validation — much easier to use, especially on mobile.

### 5.5 Make the dashboard responsive / mobile-friendly
The records table overflows on small screens. Replacing it with a card-based layout on mobile (CSS `@media` or a responsive table library) would make the app usable on a phone while gaming.

### 5.6 Add a dark mode
The app already uses a dark color palette in many places. Formalizing this as a CSS custom-property–based theme with a light/dark toggle (persisted in `localStorage`) would be a nice quality-of-life feature.

### 5.7 Improve the `About` page
The About page describes features but would benefit from screenshots, GIFs, or an animated walkthrough so new users understand the product before signing up.

---

## 6. Data & Database

### 6.1 Add database schema / seed files
There are no SQL migration or seed files in the repository. Adding a `db/` folder with `schema.sql` and `seed.sql` (or EF Core migrations) would make it trivial for contributors to spin up a local instance.

### 6.2 Add indexes on foreign keys and filter columns
Based on the query patterns, the following indexes are likely missing:
- `Records(user_id)`
- `Records(event, class_rank)`
- `Records(car_id)`
- `Builds(user_id, car_id)`

### 6.3 Implement a data archival / cleanup strategy
All deletes are soft deletes (`deleted = 1`). Over time, deleted rows accumulate. Consider a scheduled job that hard-deletes rows where `deleted = 1` and `add_date < NOW() - INTERVAL 90 DAY`, or at minimum add a manual admin endpoint for cleanup.

### 6.4 Support multiple databases via EF Core
Tying the app to MySQL via raw `MySql.Data` makes it hard to switch databases. EF Core with provider abstraction would let contributors run SQLite locally and MySQL/Postgres in production.

---

## 7. Testing

The project currently has **zero automated tests**. Introducing even a minimal test suite would dramatically improve confidence when making the multi-game changes above.

### 7.1 Backend unit tests (xUnit)
- Test `UserService` authentication logic (correct password, wrong password, deleted user)
- Test `RecordService` time validation (boundary values for min/sec/ms)
- Test `RecordRepository` sort order

### 7.2 Backend integration tests
Use `WebApplicationFactory<Program>` + an in-memory SQLite database (via EF Core) to test full HTTP round-trips for all controllers without needing a live MySQL instance.

### 7.3 Frontend component tests (Vitest + React Testing Library)
- Test `AddRecordModal` form validation and submission
- Test `Records` table filtering logic
- Test `PrivateRoute` redirect behavior

### 7.4 End-to-end tests (Playwright)
Record a happy-path E2E test: sign up → add a record → verify it appears in the table → delete it. This catches regressions that unit tests miss.

---

## 8. DevOps & Infrastructure

### 8.1 Add a CI/CD pipeline (GitHub Actions)
There is currently no `.github/workflows/` directory. A basic pipeline should:
- Run backend `dotnet build` + `dotnet test` on every pull request
- Run frontend `npm run build` + `npm test` on every pull request
- Auto-deploy to Fly.io and Vercel on merge to `main`

### 8.2 Add a `.gitignore` for build artifacts
The `api/out/` directory (compiled binaries, `.dll` files) and `react-app/node_modules/` appear to not be fully excluded. These should never be committed. Verify `.gitignore` covers `bin/`, `obj/`, `out/`, `node_modules/`, and `dist/`.

### 8.3 Store secrets in a secrets manager
API keys and database credentials are passed via environment variables, which is correct, but they should be stored in a dedicated secrets manager (GitHub Actions Secrets for CI, Fly.io Secrets for the API, Vercel Environment Variables for the frontend) rather than being passed manually.

### 8.4 Add Dependabot alerts
Enable GitHub Dependabot to automatically open pull requests for outdated npm and NuGet packages. The project currently uses Bootstrap 4 (end-of-life) and MySql.Data 9.1.0, both of which have newer alternatives.

### 8.5 Use multi-stage Docker builds consistently
The `api/Dockerfile` already uses a multi-stage build. Verify the `react-app/Dockerfile` does the same (build stage → static file serve stage) to minimize the production image size.

---

## 9. Documentation

### 9.1 Write a real README
`README.md` currently contains only `# forzatrack` (12 bytes). At minimum it should include:
- What the project does and who it's for
- Screenshots
- Local development setup instructions (prerequisites, env vars, `docker compose up`)
- How to contribute

### 9.2 Document environment variables
Create a `.env.example` file in both `api/` and `react-app/` listing every required and optional environment variable with a description and example value. This is especially important for onboarding contributors.

### 9.3 Add API documentation to Swagger
Swagger is already wired up. Adding XML doc comments to controllers and request/response models (`///`) would auto-populate Swagger UI with descriptions, making the API self-documenting.

### 9.4 Add a CHANGELOG
When making breaking changes (e.g., migrating to JWT auth, adding game support), maintain a `CHANGELOG.md` so users and contributors know what changed between versions.

---

## Quick-Win Priority Matrix

| Item | Effort | Impact | Priority |
|------|--------|--------|----------|
| Hash passwords | Low | 🔴 Critical | **Do first** |
| Remove committed credentials | Low | 🔴 Critical | **Do first** |
| Move API URL to env var | Low | Medium | High |
| Move CORS origins to config | Low | Medium | High |
| Add `Game` DB entity + API endpoints | Medium | High | High |
| Serve events/classes from API | Medium | High | High |
| Complete Builds page | Medium | High | High |
| Add DB schema/seed files | Low | Medium | Medium |
| Add JWT auth | Medium | High | Medium |
| Add GitHub Actions CI | Low | High | Medium |
| Add unit tests | Medium | High | Medium |
| Server-side pagination | Medium | Medium | Medium |
| Edit record | Low | Medium | Medium |
| Leaderboards | Medium | High | Low |
| TanStack Query | Medium | Medium | Low |
| Recharts trend charts | Medium | Medium | Low |
