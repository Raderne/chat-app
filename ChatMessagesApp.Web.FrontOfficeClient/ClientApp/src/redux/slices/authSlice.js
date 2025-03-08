import { createSlice } from "@reduxjs/toolkit";

const initialState = {
	userName: localStorage.getItem("userName") || null,
	token: localStorage.getItem("token") || null,
};

const authSlice = createSlice({
	name: "auth",
	initialState,
	reducers: {
		setValues(state, { payload }) {
			state.userName = payload.userName;
			state.token = payload.token;
			localStorage.setItem("userName", state.userName);
			localStorage.setItem("token", state.token);
		},
		logout(state) {
			state.userName = null;
			state.token = null;
			localStorage.removeItem("userName");
			localStorage.removeItem("token");
		},
	},
});

export const { setValues, logout } = authSlice.actions;
export default authSlice.reducer;
