import { Outlet, Link } from "react-router-dom";
import '../styles/Dashboard.css';

const DashLayout = () => {
  return (
    <div className="dash-container">
      <h1>DASHBOARD</h1>
      <nav>
        <Link to="records" className="dash-nav-link-records">Records</Link>
        <Link to="builds" className="dash-nav-link-builds">Builds</Link>
      </nav>
      <Outlet /> {/* This is where Records or Builds will be rendered */}
    </div>
  );
};

export default DashLayout;
