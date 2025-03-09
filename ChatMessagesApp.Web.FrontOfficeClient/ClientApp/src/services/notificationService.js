import { createAsyncThunk } from "@reduxjs/toolkit";

export const sendNotification = createAsyncThunk(
	"notification/sendNotification",
	async ({ userId, message }, { getState }) => {
		const { connection } = getState().notification;
		try {
			console.log(
				"invoke <<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
				connection,
				"<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<",
			);
			await connection.invoke("SendNotification", userId, message);
		} catch (error) {
			throw new Error(error.message);
		}
	},
);
