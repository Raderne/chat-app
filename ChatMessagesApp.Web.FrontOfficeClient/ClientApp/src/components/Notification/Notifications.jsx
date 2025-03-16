import { useSelector } from "react-redux";
import { selectNotifications } from "../../redux/selectors/notificationSelectors";
import NotificationMessage from "./NotificationMessage";

const Notifications = () => {
	const notifications = useSelector(selectNotifications);

	return (
		<div className="absolute top-16 right-0 w-80 bg-white border border-gray-300 rounded-lg shadow-lg p-4 z-50">
			{notifications.map((notification, index) => (
				<NotificationMessage
					key={index}
					Message={notification.Message}
				/>
			))}
		</div>
	);
};

export default Notifications;
