import { HttpTransportType, HubConnectionBuilder } from "@microsoft/signalr";
import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { messageReceived } from "./messagingSlice";

const initialState = {
	connectionStatus: "disconnected",
	notifications: [],
	error: null,
	poke: false,
};

const URL = import.meta.env.VITE_API_BASE_URL + "Hub";

export const initializeSignalR = createAsyncThunk(
	"notification/initializeSignalR",
	async (token, { dispatch }) => {
		try {
			const connection = new HubConnectionBuilder()
				.withUrl(URL, {
					accessTokenFactory: () => token,
					transport:
						HttpTransportType.LongPolling | HttpTransportType.ServerSentEvents,
					withCredentials: true,
					timeout: 30000,
					headers: {
						"X-Requested-With": "XMLHttpRequest",
					},
				})
				.withAutomaticReconnect({
					nextRetryDelayInMilliseconds: (retryContext) => {
						const BASE_DELAY = 1000;
						const MAX_DELAY = 10000;
						const JITTER = 500;
						if (retryContext.previousRetryCount > 3) {
							const delay = Math.min(
								BASE_DELAY * 2 ** retryContext.previousRetryCount,
								MAX_DELAY,
							);
							const jitter = Math.floor(Math.random() * JITTER);
							return delay + jitter;
						}
						return 0;
					},
				})
				.build();

			connection.on("ReceiveNotification", (notification) => {
				dispatch(notificationReceived(notification));
			});

			connection.on("receiveMessage", (message) => {
				console.log(
					">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>",
					message,
					">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>",
				);
				dispatch(messageReceived(message));
			});

			await connection.start();
			return connection;
		} catch (error) {
			console.log(error);
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
