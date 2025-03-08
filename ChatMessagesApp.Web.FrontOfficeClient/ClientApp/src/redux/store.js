import { configureStore } from "@reduxjs/toolkit";
import loadingSlice from "./slices/loadingSlice";
import authSlice from "./slices/authSlice";
import notificationSlice from "./slices/notificationSlice";
import signalRMiddleware from "./slices/middleware/signalRMiddleware";

const store = configureStore({
	reducer: {
		loading: loadingSlice,
		auth: authSlice,
		notification: notificationSlice,
	},
	middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(signalRMiddleware),
});

export default store;
