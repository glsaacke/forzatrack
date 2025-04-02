import {Link } from 'react-router-dom'
import '../styles/Home.css'

const Home = () => {
    return ( 
        <div className="home-container">
            <div className="home-overlay"></div>
            <div className="home-background"></div>
            <div className="home-content container">
                <h1>WELCOME TO FORZATRACK</h1>
                <h3>LOGIN OR SIGN UP TO VIEW YOUR DASHBOARD</h3>
                <Link to='/login' className='home-login'>GET STARTED</Link>
            </div>
        </div>
     );
}
 
export default Home;