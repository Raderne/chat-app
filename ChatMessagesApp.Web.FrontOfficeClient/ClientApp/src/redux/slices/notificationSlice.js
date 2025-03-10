import { HubConnectionBuilder } from "@microsoft/signalr";
import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";

const initialState = {
	connectionStatus: "disconnected",
	notifications: [],
	error: null,
	poke: false,
};

const URL = import.meta.env.VITE_API_BASE_URL + "notificationHub";

export const initializeSignalR = createAsyncThunk(
	"notification/initializeSignalR",
	async (token, { dispatch }) => {
		try {
			const connection = new HubConnectionBuilder()
				.withUrl(URL, { accessTokenFactory: () => token })
				.withAutomaticReconnect()
				.build();

			connection.on("ReceiveNotification", (notification) => {
				dispatch(notificationReceived(notification));
			});

			await connection.start();
			return connection;
		} catch (error) {
			throw new Error(error.message);
		}
	},
);

const notificationSlice = createSlice({
	name: "notification",
	initialState,
	reducers: {
		notificationReceived: (state, action) => {
			state.notifications.unshift(action.payload);
			state.poke = true;
		},
		resetPoke: (state) => {
			state.poke = false;
		},
	},
	extraReducers: (builder) => {
		builder
			.addCase(initializeSignalR.pending, (state) => {
				state.status = "connecting";
			})
			.addCase(initializeSignalR.fulfilled, (state, action) => {
				state.status = "connected";
				state.connection = action.payload;
			})
			.addCase(initializeSignalR.rejected, (state, action) => {
				state.status = "disconnected";
				state.error = action.error.message;
			});
	},
});

export const { notificationReceived, resetPoke } = notificationSlice.actions;

export default notificationSlice.reducer;
