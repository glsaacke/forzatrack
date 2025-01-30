import { Link } from 'react-router-dom';
import { useState } from "react";
import '../styles/Login.css'

const Login = () => {
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')

    return ( 
        <div className="login-container">
            <div className="home-overlay"></div>
            <div className="login-content">
                <h1>LOGIN</h1>
                <form>
                    <label>USERNAME</label>
                    <input type="text" required value={username} onChange={(e) => setUsername(e.target.value)}/>
                    <label>PASSWORD</label>
                    <input type="text" required value={password} onChange={(e) => setPassword(e.target.value)}/>
                    <button className='login-login'>GO</button>
                </form>
                
                <button className='login-create'>CREATE ACCOUNT</button>

            </div>
        </div>
     );
}
 
export default Login;