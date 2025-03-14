import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import { useSelector } from "react-redux";
import Loading from "./components/common/Loading";
import { lazy, Suspense } from "react";
import ProtectedRoute from "./components/common/ProtectedRoute";
import DemandsPage from "./pages/DemandsPage";

const Login = lazy(() => import("./pages/Login"));
const Dashboard = lazy(() => import("./pages/Dashboard"));

function App() {
	const isLoading = useSelector((state) => state.loading.isLoading);

	return (
		<>
			{isLoading && <Loading />}
			<BrowserRouter>
				<Routes>
					<Route
						path="/"
						element={
							<ProtectedRoute>
								<Suspense fallback={<Loading />}>
									<Dashboard />
								</Suspense>
							</ProtectedRoute>
						}
					>
						<Route
							index
							element={<DemandsPage />}
						/>
					</Route>
					<Route
						path="/login"
						element={<Login />}
					/>
				</Routes>
			</BrowserRouter>
		</>
	);
}

export default App;
