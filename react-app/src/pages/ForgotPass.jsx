import { useState } from "react";
import '../styles/Login.css'
import '../styles/ForgotPass.css'

const ForgotPass = () => {
    const [isLoading, setIsLoading] = useState(false)

    function handleOnSubmit() {
        setIsLoading(true)

        setIsLoading(false)
    }

    return ( 
        <div className="forgotpass-container">
            <div className="login-background"></div>
            <div className="home-overlay"></div>
            <div className="forgotpass-content">
                <h2>FORGOT PASSWORD</h2>
                <form onSubmit={handleOnSubmit}>
                    <label>EMAIL</label>
                    <input type="text" />
                    <button className='login-login' disabled={isLoading}>
                    {isLoading ? <div className="spinner"></div> : "SEND"}
                    </button>
                </form>
            </div>
        </div>
     );
}
 
export default ForgotPass;