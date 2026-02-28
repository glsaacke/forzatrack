import { Link } from 'react-router-dom'
import { useEffect } from 'react'
import '../styles/Home.css'

const Home = ({ setOnDashboard }) => {
    useEffect(() => {
        setOnDashboard(false)
    }, [setOnDashboard])

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