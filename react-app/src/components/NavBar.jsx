import { Link } from 'react-router-dom'
import '../styles/NavBar.css'

const NavBar = ({ onDashboard }) => {
    function handleLogout() {
        sessionStorage.removeItem("userId")
    }

    return (
        <nav className="navbar">
            <div className="navbar-links">
                <Link className='nav-logo' to='/'>FT</Link>
                {!onDashboard && <>
                    <Link to='/' className='nav-link'>HOME</Link>
                    <Link to='/about' className='nav-link'>ABOUT</Link>
                </>}
            </div>
            <div className="login-links">
                {onDashboard
                    ? <Link to='/' className='login-link' onClick={handleLogout}>LOG OUT</Link>
                    : <Link to='/login' className='login-link'>LOG IN</Link>}
            </div>
        </nav>
    );
}

export default NavBar;