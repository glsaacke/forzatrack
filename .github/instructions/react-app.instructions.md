---
applyTo: "react-app/**"
---

# ForzaTrack React App — Agent Instructions

React 18 SPA built with Vite. No state management library. Deployed to Vercel.

---

## Project Structure

```
react-app/src/
├── App.jsx                  # Router — defines all routes
├── main.jsx                 # React DOM entry point
├── pages/                   # One file per route
│   ├── Home.jsx             # Public landing page
│   ├── About.jsx            # Public about page
│   ├── Login.jsx            # Auth — login form
│   ├── Signup.jsx           # Auth — signup form
│   ├── Records.jsx          # Dashboard — lap records (main feature)
│   ├── Builds.jsx           # Dashboard — car builds (stub)
│   └── NotFound.jsx         # 404 fallback
├── components/              # Reusable UI components
│   ├── NavBar.jsx
│   ├── DashLayout.jsx       # Shared dashboard shell (nav + content slot)
│   ├── PrivateRoute.jsx     # Auth guard — redirects to /login if no session
│   ├── AddRecordModal.jsx   # Modal form for adding a new record
│   ├── AnalysisSection.jsx  # Renders all 4 analysis cards
│   └── analysis/            # Individual stat cards
│       ├── FastestCarCard.jsx
│       ├── FastestAvgCarCard.jsx
│       ├── MostConsistentCarCard.jsx
│       └── MostUsedCarCard.jsx
├── services/
│   └── api.js               # All API fetch functions
├── styles/                  # Per-component CSS files
│   ├── index.css
│   ├── App.css
│   ├── Home.css
│   ├── Login.css
│   ├── About.css
│   ├── NavBar.css
│   ├── Dashboard.css
│   ├── AddRecordModal.css
│   ├── AnalysisSection.css
│   └── analysis/
│       ├── Charts.css
│       └── TopCards.css
└── assets/
    └── Unbounded/           # Custom font files
```

---

## Routing (`App.jsx`)

```
/                      → <Home />           (public)
/about                 → <About />          (public)
/login                 → <Login />          (public)
/signup                → <Signup />         (public)
/dashboard             → redirect → /dashboard/records
/dashboard/records     → <Records />        (protected by PrivateRoute)
/dashboard/builds      → <Builds />         (protected, stub)
*                      → <NotFound />
```

Protected routes are wrapped with `<PrivateRoute>`. To add a new protected page:

```jsx
<Route path="/dashboard/newpage" element={
  <PrivateRoute>
    <NewPage />
  </PrivateRoute>
} />
```

---

## Authentication

- **No JWT / no cookies.** Session is a single `userId` value in `sessionStorage`.
- On login/signup success: `sessionStorage.setItem("userId", response.user.userId)`
- On logout: `sessionStorage.removeItem("userId")`
- `PrivateRoute` checks `sessionStorage.getItem("userId")` — if falsy, redirects to `/login`

When any component needs the current user: `const userId = sessionStorage.getItem("userId")`

---

## State Management

- **Local state only** — `useState` per component. No Redux, no Context.
- **No shared state** between pages. Each page fetches its own data.
- Data is re-fetched after mutations (e.g., after creating a record, re-call `getRecordsByUserId`).
- Loading states use a simple `loading` boolean: `const [loading, setLoading] = useState(false)`.

---

## API Service (`services/api.js`)

All API calls live here. Every function:
- Uses the native `fetch` API (no axios)
- Reads `import.meta.env.VITE_API_URL` for the base URL
- Reads `import.meta.env.VITE_API_KEY` for the `X-Api-Key` header
- Sets `Content-Type: application/json` on all requests

### Naming Convention

| Operation | Function Name Pattern | HTTP Method |
|---|---|---|
| Fetch all | `getAll{Entity}s()` | GET |
| Fetch by user | `get{Entity}sByUserId(userId)` | GET |
| Fetch by id | `get{Entity}ById(id)` | GET |
| Create | `create{Entity}(data)` | POST |
| Update | `update{Entity}(id, data)` | PUT |
| Soft delete | `set{Entity}Deleted(id)` | PUT |
| Hard delete | `delete{Entity}(id)` | DELETE |

### Example Pattern

```js
export const getRecordsByUserId = async (userId) => {
  const response = await fetch(
    `${import.meta.env.VITE_API_URL}/api/Record/GetRecordsByUserId?id=${userId}`,
    {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'X-Api-Key': import.meta.env.VITE_API_KEY,
      },
    }
  );
  return response.json();
};

export const createRecord = async (data) => {
  const response = await fetch(
    `${import.meta.env.VITE_API_URL}/api/Record/CreateRecord`,
    {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-Api-Key': import.meta.env.VITE_API_KEY,
      },
      body: JSON.stringify(data),
    }
  );
  return response.json();
};
```

---

## Page Component Pattern

Pages manage their own data lifecycle. Standard structure:

```jsx
import { useState, useEffect } from 'react';
import { getRecordsByUserId } from '../services/api';
import '../styles/Records.css';

export default function Records() {
  const [records, setRecords] = useState([]);
  const [loading, setLoading] = useState(true);
  const userId = sessionStorage.getItem('userId');

  useEffect(() => {
    const fetchData = async () => {
      const data = await getRecordsByUserId(userId);
      setRecords(data);
      setLoading(false);
    };
    fetchData();
  }, [userId]);

  // ...render
}
```

Dashboard pages should render inside `<DashLayout>` for consistent nav + layout.

---

## Component Patterns

### DashLayout
Wraps dashboard pages. Renders `<NavBar>` and a content area. Always use for pages under `/dashboard`.

```jsx
<DashLayout>
  {/* page content */}
</DashLayout>
```

### PrivateRoute
Auth guard. No props other than `children`.

```jsx
<PrivateRoute>
  <MyPage />
</PrivateRoute>
```

### AddRecordModal
Controlled by a `showModal` boolean state and an `onClose` / `onSuccess` callback pattern.

### Analysis Cards (`components/analysis/`)
Each card receives pre-filtered records as props and computes its own stat:
- `FastestCarCard` — lowest `time_min/sec/ms` combo
- `FastestAvgCarCard` — lowest average time per car
- `MostConsistentCarCard` — lowest standard deviation per car
- `MostUsedCarCard` — car with most record entries

When adding a new analysis card: create a file in `components/analysis/`, add styles to `styles/analysis/TopCards.css` or `styles/analysis/Charts.css`, and import + render in `AnalysisSection.jsx`.

---

## Styling Conventions

- **Plain CSS** — no CSS modules, no Tailwind, no styled-components.
- One `.css` file per component, named to match: `NavBar.jsx` → `NavBar.css`.
- Import at the top of the component: `import '../styles/NavBar.css'`
- The custom font `Unbounded` is loaded from `assets/Unbounded/` and defined in `index.css`.
- Dashboard styles live in `Dashboard.css`; analysis card styles in `styles/analysis/`.

---

## Environment Variables

Defined in `.env` (not committed) and `.env.example` (if present). Accessed via `import.meta.env.*`.

| Variable | Description |
|---|---|
| `VITE_API_URL` | API base URL, e.g. `http://localhost:8080` or `https://api.forzatrack.com` |
| `VITE_API_KEY` | API key sent as `X-Api-Key` on every request |

Only `VITE_`-prefixed variables are exposed to the browser by Vite.

---

## Adding a New Dashboard Feature (Checklist)

1. **`services/api.js`** — add fetch functions following the naming pattern above
2. **`pages/NewFeature.jsx`** — create page component, import `DashLayout`, manage local state
3. **`styles/NewFeature.css`** — create matching CSS file, import in the page component
4. **`App.jsx`** — register the route under `/dashboard/newfeature` inside `<PrivateRoute>`
5. **`DashLayout.jsx` / `NavBar.jsx`** — add nav link if the feature should appear in the sidebar/nav

---

## Build & Deploy

```bash
# Local dev
npm run dev          # Vite dev server on :5173

# Production build
npm run build        # Outputs to dist/

# Preview production build locally
npm run preview
```

Vercel picks up `react-app/vercel.json` which configures SPA rewrites so all paths serve `index.html`.

Docker: `react-app/Dockerfile` uses `node:18-alpine`, installs deps, and runs `npm run dev` for local container use.

---

## What Not to Do

- Do not add Redux, Zustand, or Context API unless explicitly requested.
- Do not switch to axios unless explicitly requested.
- Do not use CSS modules or Tailwind — keep plain CSS.
- Do not add TypeScript — the project is plain JSX.
- Do not add new npm packages without confirming with the user.
- Do not use `localStorage` for auth — use `sessionStorage` to match existing behavior.
