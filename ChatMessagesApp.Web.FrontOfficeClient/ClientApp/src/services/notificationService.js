import { signalRError } from "../redux/slices/notificationSlice";

export const sendNotification = async (userId, message) => {
	return async (dispatch, getState) => {
		const connection = getState().notification.connection;
		try {
			await connection.invoke("SendNotification", userId, message);
		} catch (error) {
			dispatch(signalRError(error.message));
		}
	};
};
