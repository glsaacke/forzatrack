import { Outlet, Link } from "react-router-dom";

const DashLayout = () => {
  return (
    <div>
      <h1>Dashboard</h1>
      <nav>
        <Link to="records">Records</Link> | <Link to="builds">Builds</Link>
      </nav>
      <Outlet /> {/* This is where Records or Builds will be rendered */}
    </div>
  );
};

export default DashLayout;
