import {
	createContext,
	useCallback,
	useContext,
	useEffect,
	useRef,
	useState,
} from "react";
import { useDispatch } from "react-redux";
import { initializeSignalR } from "../redux/slices/notificationSlice";

const signalRContext = createContext();

export const SignalRProvider = ({ children }) => {
	const connectionRef = useRef(null);
	const [retryCount, setRetryCount] = useState(0);
	const dispatch = useDispatch();
	const token = localStorage.getItem("token");
	const [connection, setConnection] = useState(null);

	const startConnection = useCallback(async () => {
		if (!token) {
			console.error("Token is missing. Cannot initialize SignalR connection.");
			return;
		}
		try {
			const action = await dispatch(initializeSignalR(token));
			if (initializeSignalR.fulfilled.match(action)) {
				connectionRef.current = action.payload;
				setConnection(action.payload);

				console.log(
					"<<<<<< Connection established >>>>>>>>",
					connectionRef.current,
				);

				connectionRef.current.onclose((error) => {
					if (error) {
						console.log("Connection closed with error:", error);
					}
					if (retryCount < 3) {
						setTimeout(() => {
							setRetryCount((c) => c + 1);
							startConnection();
						}, 2000);
					}
				});
			}
		} catch (error) {
			console.error("Connection failed:", error);
		}
	}, [token, retryCount, dispatch]);

	useEffect(() => {
		let isMounted = true;

		const initialize = async () => {
			if (token && isMounted) await startConnection();
		};
		initialize();

		return () => {
			isMounted = false;
			if (connectionRef.current) {
				connectionRef.current.stop();
				connectionRef.current = null;
				setConnection(null);
			}
		};
	}, [token, startConnection]);

	useEffect(() => {
		const cleanupBeforeUnload = () => {
			if (connectionRef.current) {
				connectionRef.current.stop().catch(() => {});
			}
		};

		window.addEventListener("beforeunload", cleanupBeforeUnload);
		return () =>
			window.removeEventListener("beforeunload", cleanupBeforeUnload);
	}, []);

	const handleRefresh = async () => {
		if (connectionRef.current) {
			try {
				await connectionRef.current.stop();
				connectionRef.current = null;
				setConnection(null);
				await startConnection();
			} catch (error) {
				console.error("Refresh connection error:", error);
			}
		}
	};

	const markAsRead = async (notificationIds) => {
		console.log("<<<<<<<<<<<<<< connection", connection);
		if (!connection) {
			console.error("Connection is not established.");
			return;
		}
		console.log(
			"Marking notifications as read: <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
			notificationIds,
		);
		await connection?.invoke("MarkAsRead", notificationIds);
	};

	return (
		<signalRContext.Provider
			value={{
				connection,
				handleRefresh,
				markAsRead,
			}}
		>
			{children}
		</signalRContext.Provider>
	);
};

export const useSignalR = () => useContext(signalRContext);
