import { Link, useNavigate } from 'react-router-dom';
import { useState } from "react";
import '../styles/Login.css';
import {createUser} from '../services/api';

const Signup = () => {
    const [username, setUsername] = useState('')
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [signupError, setSignupError] = useState(false)
    const [errorMessage, setErrorMessage] = useState('')

    async function handleSubmit(e){
        e.preventDefault()

        const response = await createUser(username, email, password)

        if(response.success == true){
            console.log(`hooray! ${email} and ${username} are untaken`)
            sessionStorage.setItem("userId", response.userId)

            navigate('/dashboard/records')
        }
        else{
            alert(response.message)
            // setErrorMessage(user.message)
            // setLoginError(true)
        }
    }

    return ( 
        <div className="login-container">
            <div className="login-overlay"></div>
            <div className="login-content">
                <h1>SIGN UP</h1>
                <form onSubmit={handleSubmit}>
                    <label>USERNAME</label>
                    <input type="text" required value={username} onChange={(e) => setUsername(e.target.value)}/>
                    <label>EMAIL</label>
                    <input type="text" required value={email} onChange={(e) => setEmail(e.target.value)}/>
                    <label>PASSWORD</label>
                    <input type="text" required value={password} onChange={(e) => setPassword(e.target.value)}/>
                    {signupError && <p className='login-error-message'>Error: {errorMessage}</p>}
                    <button className='login-login'>GO</button>
                </form>
            </div>
        </div>
     );
}
 
export default Signup;