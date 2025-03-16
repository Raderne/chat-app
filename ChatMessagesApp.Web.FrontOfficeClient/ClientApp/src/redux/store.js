import { configureStore } from "@reduxjs/toolkit";
import loadingSlice from "./slices/loadingSlice";
import authSlice from "./slices/authSlice";
import notificationSlice from "./slices/notificationSlice";
import messagingSlice from "./slices/messagingSlice";

const store = configureStore({
	reducer: {
		loading: loadingSlice,
		auth: authSlice,
		notification: notificationSlice,
		chat: messagingSlice,
	},
	middleware: (getDefaultMiddleware) =>
		getDefaultMiddleware({
			serializableCheck: {
				ignoredActions: ["notification/initializeSignalR/fulfilled"],
				ignoredPaths: ["notification.connection"],
			},
		}),
});

export default store;
