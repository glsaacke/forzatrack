# Searchable Car Dropdown in Add Record Modal Implementation Plan

Replace the plain `<select>` element for car selection in `AddRecordModal` with a searchable dropdown so users can easily find a car from a large list. This is a self-contained frontend change with no backend work required.

---

## Tasks

### 1. Choose an implementation approach

There are two viable options; the preferred approach is to use the native HTML `<datalist>` element (no new dependencies). A custom-built combobox is an alternative only if more control is needed.

**Option A — Native `<datalist>` (recommended)**  
Pairs an `<input type="text">` with a `<datalist>` containing all car options. The browser renders a native searchable dropdown. No new packages required.

**Option B — Custom combobox**  
Build a controlled `<input>` + filtered dropdown list using React state. More control over UX but more code.

This plan covers **Option A**. The user should be consulted before switching to Option B.

### 2. Replace the car `<select>` with an `<input>` + `<datalist>` in `AddRecordModal`
**File:** `react-app/src/components/AddRecordModal.jsx`

Currently the car selector is:

```jsx
<select className="create-car-select" required ref={selectedCar}>
    <option value='' selected>SELECT CAR</option>
    {cars.map((car) => (
        <option value={car.carId}>{car.make} {car.model} {car.year}</option>
    ))}
</select>
```

Replace with a controlled `<input>` + `<datalist>`. Because `<datalist>` only submits the displayed text (not the car ID), we need to track the selected car ID via state:

```jsx
const [carInputValue, setCarInputValue] = useState('');
const [selectedCarId, setSelectedCarId] = useState('');

function handleCarInput(e) {
    const inputText = e.target.value;
    setCarInputValue(inputText);

    const match = cars.find(
        car => `${car.make} ${car.model} ${car.year}` === inputText
    );
    setSelectedCarId(match ? match.carId : '');
}
```

JSX:
```jsx
<input
    type="text"
    list="car-options"
    value={carInputValue}
    onChange={handleCarInput}
    placeholder="Search for a car..."
    required
    className="create-car-input"
/>
<datalist id="car-options">
    {cars.map(car => (
        <option
            key={car.carId}
            value={`${car.make} ${car.model} ${car.year}`}
        />
    ))}
</datalist>
```

### 3. Use `selectedCarId` in the record submission
**File:** `react-app/src/components/AddRecordModal.jsx`

Replace `selectedCar.current.value` (the old ref) with the `selectedCarId` state variable when building `newRecord`:

```js
let newRecord = {
    userId: sessionStorage.getItem("userId"),
    carId: selectedCarId,   // was: selectedCar.current.value
    courseId: ...,
    classRank: selectedClass.current.value,
    // ...
};
```

Add a validation guard before submitting to prevent submission when no valid car is matched:

```js
if (!selectedCarId) {
    setCreateRecordErrorMessage('Please select a valid car from the list.');
    setCreateRecordError(true);
    setIsLoading(false);
    return;
}
```

### 4. Reset the car input on successful submission
**File:** `react-app/src/components/AddRecordModal.jsx`

After a successful record creation, reset both state values:

```js
setCarInputValue('');
setSelectedCarId('');
```

Remove the old `selectedCar.current.value = ''` ref reset if it is still present.

### 5. Add CSS for the car search input
**File:** `react-app/src/styles/AddRecordModal.css`

Style `.create-car-input` to match the visual weight of the existing `.create-car-select`:

```css
.create-car-input {
    width: 100%;
    padding: 0.4rem 0.6rem;
    /* match existing form element styles */
}
```

---

## Notes
- Native `<datalist>` browser support is universal in modern browsers. It does not require any new npm packages.
- The `<datalist>` approach has a known UX quirk: some browsers show the full list before any text is typed. This is acceptable given the requirement is simply to make the list searchable.
- If filtering behaviour needs to be more precise (e.g., match on any substring, not just prefix), a custom combobox component would be needed. Discuss with the user before implementing that.
- This feature is independent of the game switcher — cars are shared across FH5 and FH6 per the analysis Q&A. The full car list is always shown regardless of selected game.
