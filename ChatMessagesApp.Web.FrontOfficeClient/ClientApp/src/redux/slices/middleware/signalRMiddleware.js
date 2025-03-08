import { HubConnectionBuilder } from "@microsoft/signalr";
import {
	notificationReceived,
	signalRConnected,
	signalRConnecting,
	signalRDisconnected,
	signalRError,
} from "../notificationSlice";

let connection = null;

const signalRMiddleware = (store) => (next) => (action) => {
	const { dispatch, getState } = store;

	// Start connection when user logs in
	if (action.type === "auth/login") {
		const token = getState().auth.token;

		const startConnection = async () => {
			try {
				dispatch(signalRConnecting());

				connection = new HubConnectionBuilder()
					.withUrl(import.meta.env.VITE_API_BASE_URL + "notificationHub", {
						accessTokenFactory: () => token,
					})
					.withAutomaticReconnect()
					.build();

				connection.on("ReceiveNotification", (notification) => {
					dispatch(notificationReceived(notification));
				});
				connection.onclose(() => dispatch(signalRDisconnected()));
				connection.onreconnecting(() => dispatch(signalRConnecting()));
				connection.onreconnected(() => dispatch(signalRConnected()));

				await connection.start();
				dispatch(signalRConnected());
			} catch (error) {
				dispatch(signalRError(error.message));
			}
		};

		startConnection();

		// Stop connection when user logs out
		if (action.type === "auth/logout") {
			if (connection) {
				connection.stop();
				dispatch(signalRDisconnected());
			}
		}

		return next(action);
	}
};

export default signalRMiddleware;
