import {
	createContext,
	useCallback,
	useContext,
	useEffect,
	useRef,
	useState,
} from "react";
import { useDispatch, useSelector } from "react-redux";
import { initializeSignalR } from "../redux/slices/notificationSlice";
import { HubConnectionState } from "@microsoft/signalr";

const signalRContext = createContext();

export const SignalRProvider = ({ children }) => {
	const connectionRef = useRef(null);
	const [retryCount, setRetryCount] = useState(0);
	const dispatch = useDispatch();
	const token = useSelector((state) => state.auth.token);
	const connectionState = {
		isInitializing: false,
		isConnected: false,
	};

	const startConnection = useCallback(async () => {
		try {
			const action = await dispatch(initializeSignalR(token));
			if (initializeSignalR.fulfilled.match(action)) {
				connectionRef.current = action.payload;

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

	const safeInvoke = async (methodName, ...args) => {
		if (!connectionRef.current) {
			console.warn("Connection not initialized");
			return;
		}

		try {
			if (connectionRef.current.state === HubConnectionState.Disconnected) {
				connectionState.isInitializing = true;
				await connectionRef.current.start();
				connectionState.isInitializing = false;
			}

			console.log("Invoking method:", methodName);
			await connectionRef.current.invoke(methodName, ...args);
		} catch (error) {
			console.error("Error invoking method:", error);
			if (!connectionState.isInitializing) {
				connectionState.isConnected = false;
			}
		}
	};

	const handleRefresh = async () => {
		if (connectionRef.current) {
			try {
				await connectionRef.current.stop();
				connectionRef.current = null;
				await startConnection();
			} catch (error) {
				console.error("Refresh connection error:", error);
			}
		}
	};

	return (
		<signalRContext.Provider
			value={{
				connection: connectionRef.current,
				safeInvoke,
				handleRefresh,
			}}
		>
			{children}
		</signalRContext.Provider>
	);
};

export const useSignalR = () => useContext(signalRContext);
