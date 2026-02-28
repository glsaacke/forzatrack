import { useNavigate } from 'react-router-dom';
import { useState } from "react";
import '../styles/Login.css';
import { createUser } from '../services/api';

const Signup = () => {
    const [username, setUsername] = useState('')
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [signupError, setSignupError] = useState(false)
    const [errorMessage, setErrorMessage] = useState('')
    const [isLoading, setIsLoading] = useState(false)
    const navigate = useNavigate();

    async function handleSubmit(e) {
        e.preventDefault()
        setIsLoading(true)
        setSignupError(false)

        try {
            const response = await createUser(username, email, password)

            if (response.success) {
                sessionStorage.setItem("userId", response.user.userId)
                navigate('/dashboard/records')
            } else {
                setErrorMessage(response.message)
                setSignupError(true)
            }
        } catch {
            setErrorMessage("An unexpected error occurred. Please try again.")
            setSignupError(true)
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <div className="login-container">
            <div className="login-background"></div>
            <div className="login-overlay"></div>
            <div className="login-content">
                <h2>SIGN UP</h2>
                <form onSubmit={handleSubmit}>
                    <label>USERNAME</label>
                    <input type="text" required value={username} onChange={(e) => setUsername(e.target.value)} />
                    <label>EMAIL</label>
                    <input type="text" required value={email} onChange={(e) => setEmail(e.target.value)} />
                    <label>PASSWORD</label>
                    <input type="password" required value={password} onChange={(e) => setPassword(e.target.value)} />
                    {signupError && <p className='login-error-message'>{errorMessage}</p>}
                    <button className='login-login' disabled={isLoading}>
                        {isLoading ? <div className="spinner"></div> : "GO"}
                    </button>
                </form>
            </div>
        </div>
    );
}

export default Signup;