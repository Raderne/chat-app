/* eslint-env jest */
import { sendNotification } from "../services/SignalRService"; // Mock this service

const addDemand = async (demand) => {
	try {
		const payload = {
			title: demand.title,
			description: demand.description,
			notifyUserId: demand.notifyUserId,
		};

		const res = await fetch(URL + "api/Application/create-demand", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				Authorization: `Bearer ${demand.token}`,
			},
			body: JSON.stringify(payload),
		});

		if (res.ok) {
			const data = await res.json();
			console.log("Demand added:", data);
		}
	} catch (error) {
		console.error("Error adding demand:", error);
	}
};

jest.mock("../services/SignalRService", () => ({
	sendNotification: jest.fn(),
}));

describe("Notification Load Test", () => {
	it("should send notifications for multiple users adding demands", async () => {
		const userCount = 1000; // Simulate 1000 users
		const mockDemand = {
			title: "Test Demand",
			description: "Test Description",
			token:
				"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQ5NjhiN2ZmLTU2MjQtNGRkNS05NmY1LTgxYTliNjA2MjYyMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InJlZGFAdGVzdC50IiwiZnVsbE5hbWUiOiJyZWRhIHJlZGEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJyZWFkYSIsInN1YiI6IjQ5NjhiN2ZmLTU2MjQtNGRkNS05NmY1LTgxYTliNjA2MjYyMiIsImV4cCI6MTc0NTMyNTI1NywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MTczIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MTczIn0.1Zp85SO-vgjDyobT9hQKivp8oK_JnGCZZM_eSz3qn1M",
		};

		for (let i = 0; i < userCount; i++) {
			await addDemand(mockDemand); // Simulate adding a demand
		}

		expect(sendNotification).toHaveBeenCalledTimes(userCount);
	});
});
