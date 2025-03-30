/* eslint-disable no-unused-vars */
import { useEffect, useMemo, useState } from "react";
import SendMessage from "./forms/SendMessage";
import { getInitialsCharFromUsername } from "../utils/getInitialsCharFromFullName";
import { useSelector } from "react-redux";
import { selectMessages } from "../redux/selectors/notificationSelectors";

const URL = import.meta.env.VITE_API_BASE_URL;

const ChatBox = ({ demandId, RecipientUserId, createdBy }) => {
	const [messageData, setMessageData] = useState([]);
	const [participantsIds, setParticipantsIds] = useState([]);
	const [conversationId, setConversationId] = useState("");
	let sendToId =
		createdBy?.split(":")[1] == localStorage.getItem("userName")
			? RecipientUserId
			: createdBy?.split(":")[0];
	const messages = useSelector(selectMessages);

	useEffect(() => {
		const getMessages = async () => {
			try {
				const response = await fetch(
					`${URL}api/Application/demand/${demandId}/messages`,
					{
						method: "GET",
						headers: {
							"Content-Type": "application/json",
							Authorization: `Bearer ${localStorage.getItem("token")}`,
						},
					},
				);
				if (response.ok) {
					const data = await response.json();
					console.log(data, "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
					setParticipantsIds(data?.participantsIds);
					setConversationId(data?.id);
					const messages = data?.messages.map((msg) => {
						return {
							grade: msg?.grade || "Moi",
							userName: msg?.createdBy.split(":")[1],
							message: msg?.content,
							publishDate: new Date(msg?.created).toLocaleDateString(),
						};
					});
					setMessageData(messages);
				} else {
					throw new Error("Failed to fetch messages");
				}
			} catch (error) {
				console.error(error);
			}
		};
		getMessages();
	}, [demandId]);

	useEffect(() => {
		// messages = ["test", "test2"];

		if (messages.length === 0) return;

		setMessageData((prev) => [
			...prev,
			{
				grade: "Moi",
				userName: localStorage.getItem("userName"),
				message: messages[0],
				publishDate: new Date().toLocaleDateString(),
			},
		]);

		// Scroll to the bottom of the messages container
		setTimeout(() => {
			const messagesContainer = document.getElementById("messages");
			if (messagesContainer) {
				messagesContainer.scrollTop = messagesContainer.scrollHeight;
			}
		}, 0);
	}, [messages]);

	return (
		<>
			<div className="border-[#C9C9C9] border-[1px] rounded-[9px] p-[20px] flex flex-col gap-[20px] h-full flex-grow-1 max-h-[500px] overflow-y-auto">
				<h5 className="text-center font-bold text-[16px]">Boite de dialogue</h5>
				<p>{sendToId}</p>
				<div
					id="messages"
					className="flex-grow flex flex-col gap-[20px] overflow-y-auto [&::-webkit-scrollbar]:w-[8px]"
				>
					{messageData.map((msg, index) => (
						<div
							className="flex gap-[10px]"
							key={index}
						>
							<div className="!w-[40px] !h-[40px] rounded-full bg-[#164998] text-white flex items-center justify-center font-montserrat text-sm flex-shrink-0">
								{getInitialsCharFromUsername(msg?.userName)}
							</div>
							<div className="break-words break-keep">
								<span className="font-semibold text-[16px]">{msg?.grade}</span>{" "}
								<span className="text-[#939393] font-medium ml-[7px]">
									Publié le {msg?.publishDate}
								</span>
								<p className="break-words break-keep font-medium text-[14px]">
									{msg?.message}
								</p>
							</div>
						</div>
					))}
				</div>
				<SendMessage
					demandId={demandId}
					conversationId={conversationId}
					setMessageData={setMessageData}
				/>
			</div>
		</>
	);
};

export default ChatBox;
