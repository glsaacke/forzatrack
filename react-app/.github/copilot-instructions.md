# ForzaTrack Frontend - GitHub Copilot Instructions

## Project Overview
This is a React-based frontend application for ForzaTrack, a racing game statistics tracking application. The app uses Vite as the build tool and follows component-based architecture.

## Technology Stack
- **Framework**: React 18.3
- **Build Tool**: Vite 6.0
- **Routing**: React Router DOM 7.1
- **Styling**: Plain CSS (no framework)
- **HTTP Client**: Fetch API
- **State Management**: React useState, sessionStorage
- **Deployment**: Vercel

## Architecture Patterns

### Folder Structure
```
react-app/
├── src/
│   ├── components/          # Reusable UI components
│   ├── pages/              # Route-level page components
│   ├── services/           # API service layer
│   ├── contexts/           # React Context providers (if any)
│   ├── styles/             # CSS stylesheets
│   └── assets/             # Images, fonts, static files
├── public/                 # Public static assets
└── index.html             # HTML entry point
```

## Coding Standards

### Component Structure
```jsx
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiFunction } from '../services/api';
import '../styles/ComponentName.css';

const ComponentName = ({ prop1, prop2 }) => {
    const [state, setState] = useState(initialValue);
    const navigate = useNavigate();

    useEffect(() => {
        // Side effects
    }, [dependencies]);

    async function handleAction() {
        // Event handlers
    }

    return (
        <div className="component-container">
            {/* JSX */}
        </div>
    );
}

export default ComponentName;
```

### Naming Conventions
- **Components**: PascalCase (e.g., `NavBar`, `AddRecordModal`)
- **Files**: Match component name (e.g., `NavBar.jsx`)
- **CSS Files**: Match component name (e.g., `NavBar.css`)
- **Functions**: camelCase (e.g., `handleSubmit`, `fetchData`)
- **CSS Classes**: kebab-case (e.g., `login-container`, `main-content`)
- **Constants**: UPPER_SNAKE_CASE for module-level constants

### Import Order
1. React imports
2. Third-party libraries
3. Local components
4. Services/utilities
5. Styles/assets

## Routing Patterns

### Route Configuration
```jsx
<Routes>
    <Route path='/' element={<Home />} />
    <Route path='/about' element={<About />} />
    <Route path='/login' element={<Login />} />
    <Route path='/signup' element={<Signup />} />
    <Route path='/dashboard' element={<PrivateRoute />}>
        <Route index element={<Navigate to="records" />} />
        <Route path='records' element={<Records />} />
    </Route>
    <Route path='*' element={<NotFound />} />
</Routes>
```

### Protected Routes
Use the `PrivateRoute` component wrapper:
```jsx
import { Navigate, Outlet } from "react-router-dom";

const PrivateRoute = () => {
    const userId = sessionStorage.getItem("userId");
    return userId ? <Outlet /> : <Navigate to="/login" replace />;
};
```

## State Management

### Session Storage Pattern
- **User Authentication**: Store `userId` in sessionStorage on login
- **Check Authentication**: Read from sessionStorage
- **Logout**: Clear sessionStorage

```javascript
// Login
sessionStorage.setItem("userId", response.user.userId);

// Check auth
const userId = sessionStorage.getItem("userId");

// Logout
sessionStorage.clear();
```

### Component State
Use `useState` for local component state:
```javascript
const [email, setEmail] = useState('');
const [isLoading, setIsLoading] = useState(false);
const [error, setError] = useState(null);
```

## API Service Layer

### API Service Pattern
All API calls are centralized in `services/api.js`:

```javascript
const BASE_URL = "https://forzatrack.fly.dev/api";
const API_KEY = import.meta.env.VITE_API_KEY;

export async function apiFunction(param) {
    const response = await fetch(`${BASE_URL}/Controller/Endpoint`, {
        method: 'GET', // or 'POST', 'PUT', 'DELETE'
        headers: {
            'Content-Type': 'application/json',
            'X-Api-Key': API_KEY
        },
        body: JSON.stringify(data) // for POST/PUT
    });
    
    const result = await response.json();
    return result;
}
```

### API Headers
Always include:
- `Content-Type: application/json`
- `X-Api-Key: API_KEY` (from environment variables)

### Environment Variables
- Prefix with `VITE_` to expose to client
- Access via `import.meta.env.VITE_VARIABLE_NAME`
- Required variables:
  - `VITE_API_KEY` - Backend API authentication key

## Form Handling

### Form Pattern
```jsx
const [formField, setFormField] = useState('');
const [error, setError] = useState(false);
const [errorMessage, setErrorMessage] = useState('');
const [isLoading, setIsLoading] = useState(false);

async function handleSubmit(e) {
    e.preventDefault();
    setIsLoading(true);
    
    const response = await apiCall(formField);
    
    setIsLoading(false);
    
    if (response.success) {
        // Handle success
        navigate('/destination');
    } else {
        setErrorMessage(response.message);
        setError(true);
    }
}

return (
    <form onSubmit={handleSubmit}>
        <input 
            type="text" 
            required 
            value={formField} 
            onChange={(e) => setFormField(e.target.value)}
        />
        {error && <p className='error-message'>{errorMessage}</p>}
        <button disabled={isLoading}>
            {isLoading ? <div className="spinner"></div> : "Submit"}
        </button>
    </form>
);
```

## Navigation

### useNavigate Hook
```javascript
import { useNavigate } from 'react-router-dom';

const navigate = useNavigate();

// Programmatic navigation
navigate('/dashboard/records');
navigate('/login', { replace: true });
```

### Link Component
```jsx
import { Link } from 'react-router-dom';

<Link to="/about">About</Link>
```

## Styling Patterns

### CSS Organization
- One CSS file per component
- Place in `styles/` directory
- Import at bottom of component file
- Use BEM-like naming for clarity

### Common CSS Patterns
```css
.component-container {
    /* Container styles */
}

.component-background {
    /* Background/overlay */
}

.component-content {
    /* Main content area */
}

.component-error-message {
    /* Error styling */
}

.spinner {
    /* Loading spinner */
}
```

## Loading States

### Loading Indicator Pattern
```jsx
const [isLoading, setIsLoading] = useState(false);

async function fetchData() {
    setIsLoading(true);
    const data = await apiCall();
    setIsLoading(false);
}

return (
    <button disabled={isLoading}>
        {isLoading ? <div className="spinner"></div> : "Label"}
    </button>
);
```

## Error Handling

### Error Display Pattern
```jsx
const [error, setError] = useState(false);
const [errorMessage, setErrorMessage] = useState('');

// In async function
if (!response.success) {
    setErrorMessage(response.message);
    setError(true);
    return;
}

// In JSX
{error && <p className='error-message'>{errorMessage}</p>}
```

## Component Props

### Props Pattern
- Destructure props in function parameters
- Use clear, descriptive prop names
- Pass state setters when child needs to update parent state

```jsx
const ChildComponent = ({ data, onUpdate, isActive }) => {
    return <div>{/* ... */}</div>;
}

// Parent
<ChildComponent 
    data={myData} 
    onUpdate={handleUpdate} 
    isActive={isActive} 
/>
```

## Modal Patterns

### Modal Component
```jsx
const ModalComponent = ({ isOpen, onClose, onSubmit }) => {
    if (!isOpen) return null;
    
    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                {/* Modal content */}
                <button onClick={onClose}>Close</button>
            </div>
        </div>
    );
}
```

## Data Fetching

### useEffect for Data Fetching
```jsx
useEffect(() => {
    async function fetchData() {
        const data = await apiCall();
        setData(data);
    }
    
    fetchData();
}, [dependencies]);
```

## Best Practices

1. **Always use environment variables** for API keys and URLs
2. **Centralize API calls** in the services layer
3. **Handle loading states** for better UX
4. **Display error messages** from API responses
5. **Use sessionStorage** for simple authentication state
6. **Validate forms** before submission
7. **Disable buttons** during async operations
8. **Use semantic HTML** elements
9. **Keep components focused** - single responsibility
10. **Clean up effects** - return cleanup functions when needed
11. **Navigate after successful operations** - use `navigate()` after API success
12. **Prevent default** on form submissions
13. **Use async/await** for API calls (not .then())
14. **Conditional rendering** - show loading, error, or success states
15. **Stop propagation** on modal overlays to prevent unwanted closes

## Common Page Patterns

### Authentication Pages
- Login/Signup pages should be public
- Redirect to dashboard after successful authentication
- Store userId in sessionStorage
- Display error messages from API

### Dashboard Pages
- Wrap in PrivateRoute
- Check for userId in sessionStorage
- Fetch user-specific data on mount
- Handle logout by clearing sessionStorage

## Vite Configuration
- Dev server runs on port 5173
- Environment variables accessed via `import.meta.env`
- Fast HMR (Hot Module Replacement)
- Build output to `dist/`

## Deployment (Vercel)
- Configure in `vercel.json`
- Rewrites for SPA routing
- Environment variables set in Vercel dashboard
- Production URL: `https://forzatrack.vercel.app`

## Performance Considerations
1. Lazy load routes if needed
2. Memoize expensive computations
3. Avoid unnecessary re-renders
4. Use keys in lists
5. Optimize images in assets
