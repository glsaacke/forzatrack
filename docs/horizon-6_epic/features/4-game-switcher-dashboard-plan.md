# Game Switcher on Dashboard Implementation Plan

Add a prominent game selector to the `Records` dashboard that defaults to FH6 on load. Switching games replaces the current record view and resets the course and class filters to defaults for the selected game. The game selection is session-level only — it is not persisted to the database.

---

## Tasks

### 1. Add game state to `Records.jsx`
**File:** `react-app/src/pages/Records.jsx`

Add a `selectedGame` state variable that defaults to `"FH6"`:

```jsx
const [selectedGame, setSelectedGame] = useState('FH6');
```

### 2. Add the game switcher UI above the other filters
**File:** `react-app/src/pages/Records.jsx`

Render the game selector as the first item inside `.record-functions`, above the class and course selects. Use a visually distinct treatment (larger text, separate row, or tab-style buttons) so it is more prominent than the other filters, per the requirements from the analysis Q&A.

Example using styled buttons:

```jsx
<div className="game-switcher">
    <h3>GAME</h3>
    <div className="game-switcher-buttons">
        <button
            className={`game-btn ${selectedGame === 'FH5' ? 'active' : ''}`}
            onClick={() => setSelectedGame('FH5')}
        >
            FH5
        </button>
        <button
            className={`game-btn ${selectedGame === 'FH6' ? 'active' : ''}`}
            onClick={() => setSelectedGame('FH6')}
        >
            FH6
        </button>
    </div>
</div>
```

> Alternative: a `<select>` dropdown, though button tabs communicate the binary choice more clearly.

### 3. Reset dependent filters when the game changes
**File:** `react-app/src/pages/Records.jsx`

Add a `useEffect` that reacts to `selectedGame` changes and resets the course and class filters to their defaults:

```jsx
useEffect(() => {
    setCourseFilter(null);  // or the first course id for the new game
    setClassFilter('');
}, [selectedGame]);
```

### 4. Filter displayed records by game
**File:** `react-app/src/pages/Records.jsx`

Records belong to a game indirectly via their `courseId`. The `courses` list (fetched from the API in Feature 5) includes a `game` field. Update `handleFilterRecords` to also filter by whether the record's course belongs to the selected game:

```jsx
function handleFilterRecords() {
    const gameCourseIds = new Set(
        courses.filter(c => c.game === selectedGame).map(c => c.courseId)
    );

    let filtered = records.filter(r => gameCourseIds.has(r.courseId));

    if (classFilter !== '') {
        filtered = filtered.filter(r => r.classRank === classFilter);
    }

    if (courseFilter !== null) {
        filtered = filtered.filter(r => r.courseId === courseFilter);
    }

    setFilteredRecords(filtered);
}
```

### 5. Pass `selectedGame` to `AddRecordModal`
**File:** `react-app/src/pages/Records.jsx`

Pass `selectedGame` and `courses` as props to `AddRecordModal` so the modal can restrict its dropdowns to the active game:

```jsx
<AddRecordModal
    cars={cars}
    courses={courses}
    selectedGame={selectedGame}
    setRecords={setRecords}
/>
```

### 6. Add CSS for the game switcher
**File:** `react-app/src/styles/Dashboard.css` (or a new `GameSwitcher.css`)

Style the switcher to make it visually dominant — larger than the class/course selects. Example:

```css
.game-switcher {
    margin-bottom: 1.5rem;
}

.game-switcher-buttons {
    display: flex;
    gap: 0.5rem;
}

.game-btn {
    padding: 0.5rem 1.5rem;
    font-size: 1rem;
    font-weight: bold;
    border: 2px solid var(--accent-colour);
    background: transparent;
    color: var(--text-colour);
    cursor: pointer;
    text-transform: uppercase;
}

.game-btn.active {
    background: var(--accent-colour);
    color: var(--background-colour);
}
```

---

## Notes
- `selectedGame` defaults to `"FH6"` as specified in the scope, since the app is being prepared for that game's launch.
- `selectedGame` must be passed to `AnalysisSection` as well — this is handled in Feature 7 (Game-Aware Analysis).
- The course and class dropdowns react to `selectedGame` in Feature 5; this feature only provides the state and the UI control.
- There are no backend changes required for this feature.
