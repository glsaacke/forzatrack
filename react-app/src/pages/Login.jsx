import { Link, useNavigate } from 'react-router-dom';
import { useState } from "react";
import { authenticateUser } from '../services/api';
import '../styles/Login.css'

const Login = () => {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [loginError, setLoginError] = useState(false)
    const [errorMessage, setErrorMessage] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    const navigate = useNavigate()

    async function handleSubmit(e){
        e.preventDefault()

        setIsLoading(true)

        const response = await authenticateUser(email, password)

        setIsLoading(false) 

        if(response.success){
            console.log(response)
            sessionStorage.setItem("userId", response.user.userId)
            navigate('/dashboard/records')
        }
        else{
            setErrorMessage(response.message)
            setLoginError(true)
        }
    }

    return ( 
        <div className="login-container">
            <div className="login-background"></div>
            <div className="home-overlay"></div>
            <div className="login-content">
                <h2>LOGIN</h2>
                <form onSubmit={handleSubmit}>
                    <label>EMAIL</label>
                    <input type="text" required value={email} onChange={(e) => setEmail(e.target.value)}/>
                    <label>PASSWORD</label>
                    <input type="text" required value={password} onChange={(e) => setPassword(e.target.value)}/>
                    {loginError && <p className='login-error-message'>{errorMessage}</p>}
                    <Link to="/forgotpass" className='forgot-pass-link'>Forgot Password</Link>
                    <button className='login-login' disabled={isLoading}>
                    {isLoading ? <div className="spinner"></div> : "GO"}
                    </button>
                </form>
                
    
                <button className='login-create' onClick={() => navigate('/signup')}>CREATE ACCOUNT</button>

            </div>
        </div>
     );
}
 
export default Login;