import { Link } from 'react-router-dom';
import '../styles/Login.css'

const Login = () => {
    return ( 
        <div className="login-container">
            <div className="home-overlay"></div>
            <div className="login-content">

                <h1>LOGIN</h1>
                <div>
                    <p>USERNAME</p>

                </div>
                <div>
                    <p>PASSWORD</p>

                </div>
                <button className='login-login'>LOGIN</button>
                <button className='login-create'>CREATE ACCOUNT</button>

            </div>
        </div>
     );
}
 
export default Login;