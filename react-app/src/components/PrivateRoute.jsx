import { Navigate, Outlet } from "react-router-dom";

const PrivateRoute = () => {
  const userId = sessionStorage.getItem("userId");

  return userId ? <Outlet /> : <Navigate to="/login" replace />;
};

export default PrivateRoute;
