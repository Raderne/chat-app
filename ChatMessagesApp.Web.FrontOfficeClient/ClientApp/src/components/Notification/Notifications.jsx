import { useSelector } from "react-redux";
import { selectNotifications } from "../../redux/selectors/notificationSelectors";

const Notifications = () => {
	const notifications = useSelector(selectNotifications);

	return (
		<div className="absolute top-16 right-0 w-80 bg-white border border-gray-300 rounded-lg shadow-lg p-4">
			{notifications.map((notification) => (
				<div key={notification.id}>{notification.message}</div>
			))}
		</div>
	);
};

export default Notifications;
