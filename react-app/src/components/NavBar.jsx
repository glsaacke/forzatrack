import {Link } from 'react-router-dom'
import '../styles/NavBar.css'

const NavBar = () => {
    return ( 
        <nav className="navbar">
            <div className="navbar-links">
                <Link className = 'nav-logo' to='/'>FT</Link>
                <Link to='/' className='nav-link'>HOME</Link>
                <Link to='/' className='nav-link'>ABOUT</Link>
            </div>
            <div className="login-links">
                <Link to='/' className='login-link'>LOG IN</Link>
                <Link to='/' className='signup-link'>SIGN UP</Link>
            </div>
        </nav>
     );
}
 
export default NavBar;