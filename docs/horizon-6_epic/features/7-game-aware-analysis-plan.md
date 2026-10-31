# Game-Aware Analysis Implementation Plan

Scope the analysis section (fastest car, fastest average, most consistent, most used) so that it only processes records belonging to the currently selected game, preventing FH5 and FH6 data from mixing in the stat cards. Depends on Feature 4 (game switcher state).

---

## Tasks

### 1. Pass `selectedGame` from `Records.jsx` to `AnalysisSection`
**File:** `react-app/src/pages/Records.jsx`

`AnalysisSection` currently receives `filteredRecords` and `cars`. The `filteredRecords` array will already be game-filtered after Features 3–5 are complete (because `handleFilterRecords` filters by the game's course IDs). This means **no change to the `AnalysisSection` component signature or the individual card components is strictly required** — the analysis cards naturally operate only on what they are given in `filteredRecords`.

However, verify that this is the case:

- After Feature 5, `handleFilterRecords` filters `records` down to only those whose `courseId` belongs to a course with `game === selectedGame`.
- `filteredRecords` (the filtered output) is what gets passed to `AnalysisSection`.
- Therefore the analysis cards already only see records for the active game.

If this is confirmed, the analysis is already game-aware as a side effect of the filtering logic in Feature 5 and no additional changes are needed here.

### 2. Verify each analysis card handles an empty `filteredRecords` array gracefully

Review each card component for edge cases when no records exist for the selected game (e.g., a brand-new FH6 record set):

**Files:**
- `react-app/src/components/analysis/FastestCarCard.jsx`
- `react-app/src/components/analysis/FastestAvgCarCard.jsx`
- `react-app/src/components/analysis/MostConsistentCarCard.jsx`
- `react-app/src/components/analysis/MostUsedCarCard.jsx`

`FastestCarCard` already handles the empty case:
```jsx
if (filteredRecords && filteredRecords.length > 0) { ... }
else { setFastestCar("None") }
```

Confirm the other three cards do the same. If any card crashes or renders blank when given an empty array, add a guard:

```jsx
if (!filteredRecords || filteredRecords.length === 0) {
    // set result state to a sensible default like "None" or "N/A"
    return;
}
```

### 3. (Optional) Display the active game in the analysis section header

If desired, show which game the analysis is scoped to so users understand the context of the stats:

**File:** `react-app/src/components/AnalysisSection.jsx`

Accept an optional `selectedGame` prop and render it in the header:

```jsx
const AnalysisSection = ({ filteredRecords, cars, selectedGame }) => {
    return (
        <div className="analysis-container">
            <h3>ANALYSIS {selectedGame && <span className="analysis-game-tag">({selectedGame})</span>}</h3>
            ...
        </div>
    );
};
```

Pass `selectedGame` from `Records.jsx`:
```jsx
<AnalysisSection filteredRecords={filteredRecords} cars={cars} selectedGame={selectedGame} />
```

Add a small CSS rule for `.analysis-game-tag` to style it as a subdued label.

---

## Notes
- The core game-scoping work is done by `handleFilterRecords` in Feature 5. This feature is primarily about verification and polish.
- If Features 3–5 are implemented correctly, this feature may require very little code — the main deliverable is confirming the analysis cards behave well with an empty or game-filtered record set.
- The optional game label in the analysis header (Task 3) is a low-effort improvement worth including for clarity.
