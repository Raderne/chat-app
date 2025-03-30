import { useLocation } from "react-router-dom";
import { useSignalR } from "../context/SignalRContext";
import { useDispatch } from "react-redux";
import { useEffect, useState } from "react";
import {
	connectionStatusChanged,
	errorOccurred,
	messageReceived,
} from "../redux/slices/messagingSlice";
import ChatBox from "./ChatBox";

const URL = import.meta.env.VITE_API_BASE_URL + "api/Application/demand";

const Demand = () => {
	const [demand, setDemand] = useState();
	const location = useLocation();
	const id = location.pathname.split("/")[2];
	const { connection } = useSignalR();
	const dispatch = useDispatch();

	useEffect(() => {
		const getDemand = async () => {
			try {
				const response = await fetch(`${URL}/${id}`, {
					method: "GET",
					headers: {
						"Content-Type": "application/json",
						Authorization: `Bearer ${localStorage.getItem("token")}`,
					},
				});
				if (response.ok) {
					const data = await response.json();
					console.log(data);
					setDemand(data);
				} else {
					throw new Error("Failed to fetch demand");
				}
			} catch (error) {
				console.error(error);
			}
		};
		getDemand();
	}, [id]);

	useEffect(() => {
		if (connection) {
			console.log(
				"Connection   >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>",
				connection,
			);
			connection.on("ReceiveMessage", (message) => {
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
				connection.off("ReceiveMessage");
			};
		}
	}, [connection, dispatch]);

	return (
		<div className="">
			<h1>Demand</h1>
			<p>
				id: {id} || created by : {demand?.createdBy?.split(":")[1]}
			</p>
			<div className="flex items-center justify-center">
				<ChatBox
					demandId={id}
					RecipientUserId={demand?.toUserId}
					createdBy={demand?.createdBy}
				/>
			</div>
		</div>
	);
};

export default Demand;
