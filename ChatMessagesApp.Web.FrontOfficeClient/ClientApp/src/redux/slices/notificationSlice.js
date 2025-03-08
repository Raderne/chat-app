import { createSlice } from "@reduxjs/toolkit";

const initialState = {
	connectionStatus: "disconnected",
	notifications: [],
	error: null,
};

const notificationSlice = createSlice({
	name: "notification",
	initialState,
	reducers: {
		signalRConnecting: (state) => {
			state.connectionStatus = "connecting";
		},
		signalRConnected: (state) => {
			state.connectionStatus = "connected";
		},
		signalRDisconnected: (state) => {
			state.connectionStatus = "disconnected";
		},
		notificationReceived: (state, action) => {
			state.notifications.unshift(action.payload);
		},
		signalRError: (state, action) => {
			state.error = action.payload;
		},
	},
});

export const {
	signalRConnecting,
	signalRConnected,
	signalRDisconnected,
	notificationReceived,
	signalRError,
} = notificationSlice.actions;

export default notificationSlice.reducer;
