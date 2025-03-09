import { useSelector } from "react-redux";
import { selectNotifications } from "../../redux/selectors/notificationSelectors";

const Notifications = () => {
	const notifications = useSelector(selectNotifications);

	return (
		<div className="absolute top-16 right-0 w-80 bg-white border border-gray-300 rounded-lg shadow-lg p-4 z-50">
			{notifications.map((notification, index) => (
				<div key={index}>{notification.message}</div>
			))}
		</div>
	);
};

export default Notifications;
