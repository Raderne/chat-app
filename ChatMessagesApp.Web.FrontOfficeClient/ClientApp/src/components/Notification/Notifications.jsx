import { useSelector } from "react-redux";
import { selectNotifications } from "../../redux/selectors/notificationSelectors";
import NotificationMessage from "./NotificationMessage";
import { useEffect, useState } from "react";
import { useSignalR } from "../../context/SignalRContext";

const URL = import.meta.env.VITE_API_BASE_URL + "api/Application/notifications";

const Notifications = () => {
	let newNotifications = useSelector(selectNotifications);
	const [notifications, setNotifications] = useState([]);
	const [pageNumber, setPageNumber] = useState(1);
	const [pageSize, setPageSize] = useState(10);
	const [totalCount, setTotalCount] = useState(0);
	const [loading, setLoading] = useState(true);
	const { markAsRead } = useSignalR();

	const getNotifications = async (number, size) => {
		try {
			setLoading(true);
			const res = await fetch(`${URL}?pageNumber=${number}&pageSize=${size}`, {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${localStorage.getItem("token")}`,
				},
			});
			const data = await res.json();

			console.log(data, "res");
			if (res.ok) {
				setTotalCount(data.totalItems);
				setPageNumber(data.pageNumber);
				setPageSize(data.pageSize);
				setNotifications(data.items);
				setLoading(false);
				// Mark notifications as read
				let notificationIds = data.items
					.filter((notification) => !notification.isRead)
					.map((notification) => notification.id);
				console.log(notificationIds, "notificationIds");
				if (notificationIds.length > 0) {
					await markAsRead(notificationIds);
				}
			}
		} catch (error) {
			console.log(error);
		}
	};
	useEffect(() => {
		getNotifications(pageNumber, pageSize);
	}, [pageNumber, pageSize]);

	useEffect(() => {
		if (newNotifications.length > 0) {
			setNotifications((prev) => [...newNotifications, ...prev]);
			setPageNumber(1); // Reset to first page when new notifications are added
		}
	}, [newNotifications]);

	// const handlePageChange = (newPage) => {
	// 	if (newPage < 1 || newPage > Math.ceil(totalCount / pageSize)) return;
	// 	setPageNumber(newPage);
	// };
	// const handlePageSizeChange = (newSize) => {
	// 	setPageSize(newSize);
	// 	setPageNumber(1); // Reset to first page when page size changes
	// };

	return (
		<div className="absolute top-16 right-0 w-80 bg-white border border-gray-300 rounded-lg shadow-lg p-4 z-50">
			{loading ? (
				<div className="flex justify-center items-center h-full">
					<span className="text-gray-500">Loading...</span>
				</div>
			) : notifications.length === 0 ? (
				<div className="flex justify-center items-center h-full">
					<span className="text-gray-500">No notifications</span>
				</div>
			) : (
				<ul className="space-y-2">
					{notifications.map((notification) => (
						<li key={notification.id}>
							<NotificationMessage
								Message={notification.message}
								isRead={notification.isRead}
							/>
						</li>
					))}
				</ul>
			)}

			{/* more size */}
			{/* <div className="flex justify-between items-center mt-4">
				<select
					value={pageSize}
					onChange={(e) => handlePageSizeChange(Number(e.target.value))}
					disabled={loading}
					className="border border-gray-300 rounded px-2 py-1"
				>
					<option value={5}>5</option>
					<option value={10}>10</option>
					<option value={20}>20</option>
				</select>
				<span className="text-gray-500">Items per page</span>
			</div> */}
			{/* Pagination controls */}

			{/* <div className="flex justify-between items-center mt-4 w-full">
				<button
					className="bg-blue-500 text-white px-2 rounded h-full"
					onClick={() => handlePageChange(pageNumber - 1)}
					disabled={pageNumber === 1}
				>
					Previous
				</button>
				<span className="text-gray-500">
					Page {pageNumber} of {Math.ceil(totalCount / pageSize)}
				</span>
				<button
					className="bg-blue-500 text-white px-2 rounded h-full"
					onClick={() => handlePageChange(pageNumber + 1)}
					disabled={pageNumber * pageSize >= totalCount}
				>
					Next
				</button>
			</div> */}
		</div>
	);
};

export default Notifications;
