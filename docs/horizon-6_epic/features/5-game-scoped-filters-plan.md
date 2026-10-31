# Game-Scoped Course & Class Filters Implementation Plan

Update the course dropdown and class filter on the dashboard and the Add Record modal to be dynamically populated based on the selected game. Courses are fetched from the API filtered by game; classes are hardcoded per game (FH5: X, S2, S1, A, B, C, D / FH6: R, S2, S1, A, B, C, D). Depends on Feature 1 (Course API), Feature 3 (CourseId on records), and Feature 4 (game switcher state).

---

## Tasks

### 1. Add the `getCoursesByGame` API function
**File:** `react-app/src/services/api.js`

```js
export async function getCoursesByGame(game) {
    const response = await fetch(
        `${BASE_URL}/Course/GetCoursesByGame?game=${encodeURIComponent(game)}`,
        {
            method: 'GET',
            headers: getAuthHeaders(),
        }
    );
    return response.json();
}
```

### 2. Fetch courses when the game changes in `Records.jsx`
**File:** `react-app/src/pages/Records.jsx`

Import `getCoursesByGame` and add a `courses` state. Whenever `selectedGame` changes, fetch the matching courses:

```jsx
const [courses, setCourses] = useState([]);

useEffect(() => {
    async function fetchCourses() {
        const data = await getCoursesByGame(selectedGame);
        setCourses(data);
        setCourseFilter(null);
        setClassFilter('');
    }
    fetchCourses();
}, [selectedGame]);
```

Remove the `getAllCourses` (or equivalent) call from the initial mount `useEffect` if it was added as a placeholder in earlier features.

### 3. Define per-game class lists
**File:** `react-app/src/pages/Records.jsx`

Hardcode the class lists as a constant at the top of the file:

```js
const CLASS_OPTIONS = {
    FH5: ['X', 'S2', 'S1', 'A', 'B', 'C', 'D'],
    FH6: ['R', 'S2', 'S1', 'A', 'B', 'C', 'D'],
};
```

### 4. Render the dynamic class dropdown in `Records.jsx`
**File:** `react-app/src/pages/Records.jsx`

Replace the hardcoded class `<select>` options with a map over `CLASS_OPTIONS[selectedGame]`:

```jsx
<select className="form-select" value={classFilter} onChange={(e) => setClassFilter(e.target.value)}>
    <option value="">All Classes</option>
    {CLASS_OPTIONS[selectedGame].map(cls => (
        <option key={cls} value={cls}>{cls}</option>
    ))}
</select>
```

### 5. Render the dynamic course dropdown in `Records.jsx`
**File:** `react-app/src/pages/Records.jsx`

Replace the old hardcoded event `<select>` with a course dropdown driven by the fetched `courses` list:

```jsx
<select className="form-select" value={courseFilter ?? ''} onChange={(e) => {
    const val = e.target.value;
    setCourseFilter(val === '' ? null : parseInt(val, 10));
}}>
    <option value="">All Courses</option>
    {courses.map(course => (
        <option key={course.courseId} value={course.courseId}>{course.name}</option>
    ))}
</select>
```

### 6. Update `AddRecordModal` to accept and use game-scoped props
**File:** `react-app/src/components/AddRecordModal.jsx`

The modal already receives `courses` and `selectedGame` as props (passed in Feature 4). Use them here:

- Render the course dropdown from the `courses` prop (not a hardcoded list).
- Render the class dropdown from `CLASS_OPTIONS[selectedGame]`.

```jsx
const AddRecordModal = ({ cars, courses, selectedGame, setRecords }) => {
    // ...

    // In the JSX:
    <select required ref={selectedCourse}>
        <option value=''>SELECT COURSE</option>
        {courses.map(course => (
            <option key={course.courseId} value={course.courseId}>{course.name}</option>
        ))}
    </select>

    <select required ref={selectedClass}>
        <option value=''>SELECT CLASS</option>
        {CLASS_OPTIONS[selectedGame].map(cls => (
            <option key={cls} value={cls}>{cls}</option>
        ))}
    </select>
};
```

Move the `CLASS_OPTIONS` constant to `api.js` or a shared constants file so both `Records.jsx` and `AddRecordModal.jsx` can import it without duplication.

### 7. Ensure `handleFilterRecords` in `Records.jsx` re-runs on course/game changes

`handleFilterRecords` should be in the dependency array of a `useEffect` that triggers when `selectedGame`, `courseFilter`, `classFilter`, or `records` changes. Confirm the existing `useEffect` watching `[classFilter, records, eventFilter]` is updated to include `courseFilter` and that `selectedGame`-driven re-filtering is also triggered when courses reload.

---

## Notes
- The `CLASS_OPTIONS` constant should be a shared module (e.g. `react-app/src/constants/gameClasses.js`) to avoid duplicating it in `Records.jsx` and `AddRecordModal.jsx`.
- When the game switches and `courses` reloads, `courseFilter` resets to `null` (all courses), which is the correct default behaviour.
- CPU difficulty tiers are the same across both games, so the `selectedCpuDiff` select in `AddRecordModal` does not need to change.
- There are no backend changes required for this feature beyond what was already built in Feature 1.
