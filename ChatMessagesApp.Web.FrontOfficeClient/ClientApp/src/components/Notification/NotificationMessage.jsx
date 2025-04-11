const NotificationMessage = ({ Message, isRead }) => {
	return (
		<div
			className="text-sm"
			style={{ color: isRead ? "gray" : "black" }}
		>
			{isRead ? (
				<span>X {Message}</span>
			) : (
				<span className="font-bold">{Message}</span>
			)}
		</div>
	);
};

export default NotificationMessage;
