# Course Entity & API Implementation Plan

Add a `Course` database table and a full CRUD API so courses (formerly called events) can be managed without direct database access. Each course has a name and a game tag (`"FH5"`, `"FH6"`, etc.).

---

## Tasks

### 1. Create the `Courses` table in MySQL

Run the following DDL against the production database before deploying the API changes:

```sql
CREATE TABLE Courses (
    course_id   INT          NOT NULL AUTO_INCREMENT,
    name        VARCHAR(100) NOT NULL,
    game        VARCHAR(20)  NOT NULL,
    deleted     INT          NOT NULL DEFAULT 0,
    PRIMARY KEY (course_id)
);
```

### 2. Create the domain model
**File:** `api/core/models/Course.cs`

```csharp
namespace api.core.models
{
    public class Course
    {
        public int? CourseId { get; set; }
        public string Name { get; set; }
        public string Game { get; set; }
        public int Deleted { get; set; }
    }
}
```

### 3. Register `Course` in `AppDbContext`
**File:** `api/core/data/AppDbContext.cs`

Add the `DbSet` property:
```csharp
public DbSet<Course> Courses { get; set; }
```

Add entity configuration inside `OnModelCreating`:
```csharp
modelBuilder.Entity<Course>(e =>
{
    e.ToTable("Courses");
    e.HasKey(c => c.CourseId);
    e.Property(c => c.CourseId).HasColumnName("course_id");
    e.Property(c => c.Name).HasColumnName("name");
    e.Property(c => c.Game).HasColumnName("game");
    e.Property(c => c.Deleted).HasColumnName("deleted");
    e.HasQueryFilter(c => c.Deleted == 0);
});
```

### 4. Create the request DTO
**File:** `api/core/controllers/models/CourseRequest.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace api.core.controllers.models
{
    public class CourseRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Game { get; set; }
    }
}
```

### 5. Create the repository interface
**File:** `api/core/services/CourseService/ICourseRepository.cs`

```csharp
using api.core.models;

namespace api.core.services.CourseService
{
    public interface ICourseRepository
    {
        List<Course> GetAllCourses();
        Course GetCourseById(int id);
        List<Course> GetCoursesByGame(string game);
        void CreateCourse(Course course);
        bool UpdateCourse(Course course, int id);
        bool SetCourseDeleted(int id);
        void DeleteCourse(int id);
    }
}
```

### 6. Create the service interface
**File:** `api/core/services/CourseService/ICourseService.cs`

Mirror the repository interface exactly (same methods, same signatures).

### 7. Create the repository implementation
**File:** `api/core/services/CourseService/CourseRepository.cs`

Follow the same EF Core pattern used by `CarRepository.cs` and `RecordRepository.cs`. Inject `AppDbContext` via the constructor.

```csharp
using api.core.models;
using api.core.data;

namespace api.core.services.CourseService
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Course> GetAllCourses() =>
            _context.Courses.ToList();

        public Course GetCourseById(int id) =>
            _context.Courses.FirstOrDefault(c => c.CourseId == id);

        public List<Course> GetCoursesByGame(string game) =>
            _context.Courses.Where(c => c.Game == game).ToList();

        public void CreateCourse(Course course) {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public bool UpdateCourse(Course course, int id) {
            var existing = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (existing == null) return false;
            existing.Name = course.Name;
            existing.Game = course.Game;
            return _context.SaveChanges() > 0;
        }

        public bool SetCourseDeleted(int id) {
            var existing = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (existing == null) return false;
            existing.Deleted = 1;
            return _context.SaveChanges() > 0;
        }

        public void DeleteCourse(int id) {
            var course = new Course { CourseId = id };
            _context.Courses.Attach(course);
            _context.Courses.Remove(course);
            _context.SaveChanges();
        }
    }
}
```

### 8. Create the service implementation
**File:** `api/core/services/CourseService/CourseService.cs`

Delegate all method calls to `ICourseRepository`. Follow the same pattern as `CarService.cs`.

### 9. Create the controller
**File:** `api/core/controllers/CourseController.cs`

```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    // ...

    [HttpGet("GetAllCourses")]
    public IActionResult GetAllCourses() { ... }

    [HttpGet("GetCourseById/{id}")]
    public IActionResult GetCourseById(int id) { ... }

    [HttpGet("GetCoursesByGame")]
    public IActionResult GetCoursesByGame([FromQuery] string game) { ... }

    [HttpPost("CreateCourse")]
    public IActionResult CreateCourse([FromBody] CourseRequest request) { ... }

    [HttpPut("UpdateCourse/{id}")]
    public IActionResult UpdateCourse(int id, [FromBody] CourseRequest request) { ... }

    [HttpPut("SetCourseDeleted/{id}")]
    public IActionResult SetCourseDeleted(int id) { ... }

    [HttpDelete("DeleteCourse/{id}")]
    public IActionResult DeleteCourse(int id) { ... }
}
```

Map each `CourseRequest` to a `Course` model before passing it to the service (do not pass the request DTO directly).

### 10. Register in `Program.cs`
**File:** `api/Program.cs`

Add two `AddScoped` registrations alongside the existing ones:

```csharp
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
```

---

## Notes
- The `HasQueryFilter` on the `Course` entity ensures soft-deleted courses are never returned by default — no need to add `WHERE deleted = 0` in every query.
- `GetCoursesByGame` is the key read endpoint for the frontend; it will be called every time the user switches games.
- No frontend changes are required for this feature — the API is the deliverable. Frontend integration happens in features 4 and 5.
