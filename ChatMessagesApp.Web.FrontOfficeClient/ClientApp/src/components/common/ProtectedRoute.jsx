import { useDispatch } from "react-redux";
import { logout } from "../../redux/slices/authSlice";
import { Navigate } from "react-router-dom";

const ProtectedRoute = ({ children }) => {
	const dospatch = useDispatch();
	const token = localStorage.getItem("token") || "";

	if (!token) {
		dospatch(logout());
		return (
			<Navigate
				to="/login"
				replace
			/>
		);
	} else {
		return children;
	}
};

export default ProtectedRoute;
