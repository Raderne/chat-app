import { createSlice } from "@reduxjs/toolkit";

const initialState = {
	messages: [],
	status: "disconnected",
	error: null,
};

const messagingSlice = createSlice({
	name: "messaging",
	initialState,
	reducers: {
		messageReceived: (state, action) => {
			state.messages.unshift(action.payload);
		},
		connectionStatusChanged: (state, action) => {
			state.status = action.payload;
		},
		errorOccurred: (state, action) => {
			state.error = action.payload;
		},
	},
});

export const { messageReceived, connectionStatusChanged, errorOccurred } =
	messagingSlice.actions;

export default messagingSlice.reducer;
