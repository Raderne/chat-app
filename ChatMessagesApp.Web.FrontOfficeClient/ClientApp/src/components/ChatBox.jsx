/* eslint-disable no-unused-vars */
import { useState } from "react";
import SendMessage from "./forms/SendMessage";
import { getInitialsCharFromUsername } from "../utils/getInitialsCharFromFullName";

const ChatBox = ({ demandId, RecipientUserId, createdBy }) => {
	const [messageData, setMessageData] = useState([]);
	let sendToId =
		createdBy?.split(":")[1] == localStorage.getItem("userName")
			? RecipientUserId
			: createdBy?.split(":")[0];

	return (
		<>
			<div className="border-[#C9C9C9] border-[1px] rounded-[9px] p-[20px] flex flex-col gap-[20px] h-full flex-grow-1">
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
					sendTo={sendToId}
				/>
			</div>
		</>
	);
};

export default ChatBox;
