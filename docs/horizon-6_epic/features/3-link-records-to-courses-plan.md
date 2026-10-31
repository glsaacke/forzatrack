# Link Records to Courses Implementation Plan

Replace the free-text `Event` string on the `Record` model with a `CourseId` foreign key that references the `Courses` table. This makes records game-aware through their course association and removes the risk of typos or unrecognised event names. Requires Feature 1 (Course entity) and Feature 2 (migration) to be complete first.

---

## Tasks

### 1. Make `course_id` non-nullable in the database

After confirming that all records have been successfully assigned a `course_id` in Feature 2, enforce the FK constraint:

```sql
ALTER TABLE Records
    MODIFY COLUMN course_id INT NOT NULL;
```

Optionally drop the old `event` column once the application code no longer references it:

```sql
ALTER TABLE Records DROP COLUMN event;
```

> Drop the `event` column only after the new API is deployed and verified in production.

### 2. Update the `Record` domain model
**File:** `api/core/models/Record.cs`

Replace the `Event` property with `CourseId`:

```csharp
public class Record
{
    public int? RecordId { get; set; }
    public int UserId { get; set; }
    public int CarId { get; set; }
    public int CourseId { get; set; }   // replaces: public string Event { get; set; }
    public string ClassRank { get; set; }
    public int TimeMin { get; set; }
    public int TimeSec { get; set; }
    public int TimeMs { get; set; }
    public string CpuDiff { get; set; }
    public DateTime AddDate { get; set; }
    public int Deleted { get; set; }
}
```

### 3. Update the `AppDbContext` mapping for `Record`
**File:** `api/core/data/AppDbContext.cs`

Replace the `event` property mapping with `course_id`:

```csharp
// Remove:
e.Property(r => r.Event).HasColumnName("event");

// Add:
e.Property(r => r.CourseId).HasColumnName("course_id");
```

### 4. Update the `RecordRequest` DTO
**File:** `api/core/controllers/models/RecordRequest.cs`

Replace `Event` with `CourseId`:

```csharp
// Remove:
[Required]
[StringLength(100)]
public string Event { get; set; }

// Add:
[Required]
[Range(1, int.MaxValue)]
public int CourseId { get; set; }
```

### 5. Update `RecordRepository.CreateRecord`
**File:** `api/core/services/RecordService/RecordRepository.cs`

The `CreateRecord` method sets `AddDate` before saving. No changes are needed to the repository logic itself — EF Core will map `CourseId` to `course_id` automatically based on the updated `AppDbContext` configuration.

Verify that `UpdateRecord` also updates `CourseId` in place of `Event`:

```csharp
existing.CourseId = record.CourseId;
// Remove: existing.Event = record.Event;
```

### 6. Update `RecordController.CreateRecord` and `UpdateRecord`
**File:** `api/core/controllers/RecordController.cs`

Wherever a `RecordRequest` is mapped to a `Record`, replace `Event` with `CourseId`:

```csharp
// In CreateRecord and UpdateRecord actions:
var record = new Record
{
    UserId    = request.UserId,
    CarId     = request.CarId,
    CourseId  = request.CourseId,   // was: Event = request.Event
    ClassRank = request.ClassRank,
    TimeMin   = request.TimeMin,
    TimeSec   = request.TimeSec,
    TimeMs    = request.TimeMs,
    CpuDiff   = request.CpuDiff,
};
```

### 7. Update the frontend API service
**File:** `react-app/src/services/api.js`

In `createRecord`, replace the `event` field with `courseId`:

```js
// In the record payload:
courseId: selectedCourse,   // was: event: selectedEvent.current.value
```

### 8. Update `AddRecordModal.jsx`
**File:** `react-app/src/components/AddRecordModal.jsx`

- Remove the hardcoded event `<select>` element.
- Replace it with a course dropdown populated from the API (this will be fully wired in Feature 5 — for now, accept `courses` as a prop and render them).
- Update `newRecord` construction to use `courseId` instead of `event`.

```jsx
// Replace the event select with:
<select required ref={selectedCourse}>
    <option value=''>SELECT COURSE</option>
    {courses.map((course) => (
        <option key={course.courseId} value={course.courseId}>
            {course.name}
        </option>
    ))}
</select>
```

### 9. Update `Records.jsx` filters
**File:** `react-app/src/pages/Records.jsx`

- Remove the hardcoded event `<select>` and its options.
- Replace `eventFilter` state with `courseFilter` (a `courseId` integer or `null` for all).
- Update `handleFilterRecords` to filter by `record.courseId === courseFilter` instead of `record.event === eventFilter`.
- The course dropdown will be populated from the API in Feature 5.

### 10. Update the records table display
**File:** `react-app/src/pages/Records.jsx`

The table currently renders `{record.event}` in the Event column. Replace this with a lookup of the course name using a `courses` list:

```jsx
// Add a helper alongside displayCar:
function displayCourse(courseId) {
    return courses.find(c => c.courseId === courseId)?.name ?? 'Unknown';
}

// In the table row:
<td>{displayCourse(record.courseId)}</td>
```

---

## Notes
- This is the largest breaking change of the epic — the `Event` field is removed from both the API and frontend simultaneously. Plan to deploy the API and frontend together.
- The `Records.jsx` course dropdown and `AddRecordModal.jsx` course select will be hardcoded to FH5 courses temporarily until Feature 5 makes them game-aware.
- The `event` DB column should only be dropped after the new build is confirmed stable in production.
