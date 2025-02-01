import { Navigate } from "react-router-dom";

const PrivateRoute = ({ element }) => {
  const userId = sessionStorage.getItem("userId");

  return userId ? element : <Navigate to="/login" replace />;
};

export default PrivateRoute;
