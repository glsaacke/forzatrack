import { Link } from 'react-router-dom';
import { useState } from "react";
import { authenticateUser } from '../services/api';
import '../styles/Login.css'

const Login = () => {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [loginError, setLoginError] = useState(false)
    const [errorMessage, setErrorMessage] = useState('')

    async function handleSubmit(e){
        e.preventDefault()

        const user = await authenticateUser(email, password)

        if(user.success){
            console.log(`hooray! ${email} is a valid login`)
        }
        else{
            setErrorMessage(user.message)
            setLoginError(true)
        }
    }

    return ( 
        <div className="login-container">
            <div className="home-overlay"></div>
            <div className="login-content">
                <h1>LOGIN</h1>
                <form onSubmit={handleSubmit}>
                    <label>USERNAME</label>
                    <input type="text" required value={email} onChange={(e) => setEmail(e.target.value)}/>
                    <label>PASSWORD</label>
                    <input type="text" required value={password} onChange={(e) => setPassword(e.target.value)}/>
                    {loginError && <p className='login-error-message'>Error: {errorMessage}</p>}
                    <button className='login-login'>GO</button>
                </form>
                
    
                <button className='login-create'>CREATE ACCOUNT</button>

            </div>
        </div>
     );
}
 
export default Login;