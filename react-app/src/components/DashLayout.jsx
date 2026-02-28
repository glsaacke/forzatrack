import { Outlet } from "react-router-dom";
import '../styles/Dashboard.css';

const DashLayout = () => {
  return (
    <div>
      <Outlet />
    </div>
  );
};

export default DashLayout;
