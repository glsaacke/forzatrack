import '../styles/About.css'

const About = () => {
    return ( 
        <div>           
        <div className="about-background"></div>
        <div className="about-overlay"></div>
            <div className="about-container">
                <div className="about-content">
                    <h2>ABOUT</h2>
                    <div className="about-section">
                        <h3>TRACK YOUR FORZA RECORDS</h3>
                        <p>ForzaTrack is the ultimate racing tracker for Forza Horizon 5, built for racers looking to track their best runs, fine-tune builds, and improve their performance over time. Whether you're a casual player or a dedicated competitor, this platform helps you log and compare your results effortlessly.</p>
                    </div>
                    <div className="about-section">
                        <h3>FEATURES</h3>
                        <div className="about-section-list">
                            <p>• Personalized Dashboard: Log in to access all your logged efforts in one place.</p>
                            <p>• Track Your Best Times: Store and compare lap records across various courses and cars.</p>
                            <p>• Filter & Compare: Sort your best times by track or car class.</p>
                        </div>
                    </div>
                    <div className="about-section">
                        <h3>HOW IT WORKS</h3>
                        <div className="about-section-list">
                            <p>Sign Up & Log In: Create an account to store your records.</p>
                            <p>Record Your Times: Enter key information like time, car, course, and CPU difficulty.</p>
                            <p>Compare & Improve: View all your times in one place and find ways to improve your performance.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
     );
}
 
export default About;