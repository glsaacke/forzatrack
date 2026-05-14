import {Link, useNavigate } from 'react-router-dom'
import '../styles/NavBar.css'

const NavBar = ({onDashboard}) => {
    const navigate = useNavigate()

    function handleLogout() {
        sessionStorage.removeItem("token")
        sessionStorage.removeItem("userId")
        navigate('/')
    }

    return ( 
        <nav className="navbar">
            <div className="navbar-links">
                <Link className = 'nav-logo' to='/'>FT</Link>
                {onDashboard ? null : <>
                <Link to='/' className='nav-link'>HOME</Link>
                <Link to='/about' className='nav-link'>ABOUT</Link>
                </> }
            </div>
            <div className="login-links">
                {onDashboard ? <button onClick={handleLogout} className='login-link'>LOG OUT</button> : <Link to='/login' className='login-link'>LOG IN</Link>}            
            </div>
        </nav>
     );
}
 
export default NavBar;