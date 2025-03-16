import { useLocation } from "react-router-dom";
import { useSignalR } from "../context/SignalRContext";
import { useDispatch } from "react-redux";
import { useEffect } from "react";
import {
	connectionStatusChanged,
	errorOccurred,
	messageReceived,
} from "../redux/slices/messagingSlice";
import ChatBox from "./ChatBox";

const Demand = () => {
	const location = useLocation();
	const id = location.pathname.split("/")[2];
	const recipientId = location.pathname.split("/")[3];
	const { connection } = useSignalR();
	const dispatch = useDispatch();

	useEffect(() => {
		if (connection) {
			connection.on("receiveMessage", (message) => {
				console.log(
					">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>",
					message,
					">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>",
				);
				dispatch(messageReceived(message));
			});

			connection
				.start()
				.then(() => {
					dispatch(connectionStatusChanged("connected"));
				})
				.catch((error) => {
					dispatch(errorOccurred(error.toString()));
				});

			return () => {
				connection.off("receiveMessage");
			};
		}
	}, [connection, dispatch]);

	return (
		<div className="">
			<h1>Demand</h1>
			<p>
				id: {id} || recipient Id : {recipientId}
			</p>
			<div className="min-w-screen min-h-screen">
				<ChatBox
					RecipientUserId={recipientId}
					demandId={id}
				/>
			</div>
		</div>
	);
};

export default Demand;
