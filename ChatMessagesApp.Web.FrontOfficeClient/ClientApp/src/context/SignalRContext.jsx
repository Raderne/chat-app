import { HubConnectionBuilder } from "@microsoft/signalr";
import { createContext, useContext, useEffect, useRef } from "react";
import { useDispatch, useSelector } from "react-redux";
import { initializeSignalR } from "../redux/slices/notificationSlice";

const signalRContext = createContext();

const URL = import.meta.env.VITE_API_BASE_URL + "notificationHub";

export const SignalRProvider = ({ children }) => {
	const connectionRef = useRef(null);
	const dispatch = useDispatch();
	const token = useSelector((state) => state.auth.token);

	useEffect(() => {
		if (token) {
			dispatch(initializeSignalR(token)).then((action) => {
				if (initializeSignalR.fulfilled.match(action)) {
					connectionRef.current = action.payload; // Store the connection in a ref
				}
			});
		}

		return () => {
			if (connectionRef.current) {
				connectionRef.current.stop();
			}
		};
	}, [token, dispatch]);

	const stopConnection = async () => {
		if (connectionRef.current) {
			await connectionRef.current.stop();
			connectionRef.current = null;
		}
	};

	const invoke = async (methodName, ...args) => {
		if (connectionRef.current) {
			await connectionRef.current.invoke(methodName, ...args);
		}
	};

	return (
		<signalRContext.Provider
			value={{
				connection: connectionRef.current,
				stopConnection,
				invoke,
			}}
		>
			{children}
		</signalRContext.Provider>
	);
};

export const useSignalR = () => useContext(signalRContext);
