import { Link } from 'react-router-dom';
import '../styles/Login.css'

const Login = () => {
    return ( 
        <div className="login-container">
            <div className="login-content">

                <h1>LOGIN</h1>
                <div>
                    <p>USERNAME</p>

                </div>
                <div>
                    <p>PASSWORD</p>

                </div>
                <button>LOGIN</button>
                <button>CREATE ACCOUNT</button>

            </div>
        </div>
     );
}
 
export default Login;