import { HttpTransportType, HubConnectionBuilder } from "@microsoft/signalr";

export const CreateSignalRConnection = (URL, token) => {
	const connection = new HubConnectionBuilder()
		.withUrl(URL, {
			accessTokenFactory: () => token,
			transport:
				HttpTransportType.LongPolling | HttpTransportType.ServerSentEvents,
			withCredentials: true,
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

	return connection;
};
