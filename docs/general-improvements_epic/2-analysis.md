# General Improvements Analysis

## Questions

Q: Passwords are currently stored and transmitted as plain text. Is there an existing user base that would need a migration path (e.g., force a password reset on next login), or is this a greenfield situation where all accounts can simply be re-created?
A: There should only be 3 or so users currently in the database. The gs.xc.02@gmail.com user has all my important data on it- it's critical that this account remains usable. Right now, I am the owner of all the accounts in the project, and I can see the plain text paswords in the database, so I would perfer if the current passwords can remain the same and just be hashed. I'm open to other suggestions.

Q: For the login overhaul, is the preference to stay sessionStorage-based (just add proper hashing + a signed token), or move to a more standard flow using HTTP-only cookies and refresh tokens?
A: I'm open to suggestions. It doesn't need to be extremely complex or secure since I'm not keeping any super sensitive information, but it needs to be better than what I currently have (plaintext passwords)

Q: The API key is embedded in the frontend bundle and is therefore publicly visible to anyone who inspects network traffic. Should the long-term goal be to eliminate the need for a client-side API key entirely, or is it acceptable to keep a rotating/public key as a basic anti-scraping measure?
A: I'm open to suggestions here as well. Whatever the best balance is for a simple project with low stakes but is public.

Q: The custom `DotEnv.cs` middleware and the `DotNetEnv` NuGet package are both loaded at startup. Is the custom one intentionally kept, or is it safe to remove it in favour of `DotNetEnv`?
A: It's safe to remove as long as the project is using the package instead

Q: For the EF Core migration, should the existing raw SQL schema and column names be preserved as-is (table-per-entity mapping), or is this an opportunity to revise the schema design at the same time?
A: Do not change current schema unless major changes are needed. It is ok to add to the schema (columns and such) if it is cleared by me.

Q: Should the EF Core migration replace all existing repositories at once, or be done incrementally entity by entity?
A: All at once is fine - they aren't very complex

Q: Are there any plans to add an admin role or multi-tenancy concerns (e.g., users being able to view each other's records), or will this remain strictly per-user private data?
A: Those are potential plans for the future, but will not be implemented at this time. Feel free to code in a way that would make it easy to implement something like this in the future though.

---

## Feature Suggestions

### Password Hashing
- Hash passwords at rest using a standard algorithm (e.g., BCrypt). Also move credential transmission off a GET query string and onto a POST body so passwords are never logged in server access logs.
Approve: true

### JWT Authentication
- Replace the current `sessionStorage` userId pattern with short-lived JWTs (returned on login, sent as a Bearer token). This gives stateless, verifiable session data without storing a raw user ID on the client.
Approve: true

### EF Core Migration
- Replace the raw ADO.NET repositories with EF Core using the existing MySQL schema. This reduces boilerplate, adds compile-time query safety, and makes future schema changes easier to manage with migrations.
Approve: true

### Input Validation
- Add server-side validation to all request DTOs (e.g., required fields, max lengths, valid email format). Currently the API accepts and persists whatever is sent.
Approve: true

### Rate Limiting on Auth Endpoints
- Apply a request rate limit to the login and signup endpoints to protect against brute-force and credential-stuffing attacks.
Approve: true

### Email Verification on Signup
- Send a verification email when a new account is created, and prevent login until the address is confirmed. Relevant if the site is going public.
Approve: false

### Password Reset Flow
- Allow users to reset their password via a time-limited email link, rather than requiring manual database intervention.
Approve: false

### Centralised Error Handling
- Add a global exception handler that returns consistent `ProblemDetails` responses instead of unhandled 500s leaking stack traces.
Approve: false

---

## Follow-up Questions

Q: For the password migration — the safest approach is a one-time script that reads the existing plain-text passwords from the DB and writes back their BCrypt hashes before the new API goes live. Is that acceptable, or do you have a preferred method (e.g., migrate on first login by detecting unhashed passwords)?
A: That is fine

Q: The approved Password Reset Flow requires email infrastructure (an SMTP server or a transactional email service like SendGrid/Resend). Do you have a preference, or should we scope this feature out of the current epic and revisit it separately?
A: Lets visit this later. I changed the approval of this feature to false.
