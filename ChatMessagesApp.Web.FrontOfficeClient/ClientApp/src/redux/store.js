import { configureStore } from "@reduxjs/toolkit";
import loadingSlice from "./slices/loadingSlice";
import authSlice from "./slices/authSlice";
import notificationSlice from "./slices/notificationSlice";

const store = configureStore({
	reducer: {
		loading: loadingSlice,
		auth: authSlice,
		notification: notificationSlice,
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
