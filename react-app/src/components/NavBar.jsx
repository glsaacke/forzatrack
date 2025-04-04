import {Link } from 'react-router-dom'
import '../styles/NavBar.css'

const NavBar = ({onDashboard}) => {
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
                {onDashboard ? <Link to='/' className='login-link'>LOG OUT</Link> : <Link to='/login' className='login-link'>LOG IN</Link>}            
            </div>
        </nav>
     );
}
 
export default NavBar;