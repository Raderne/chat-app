import { Button, Col, notification, Row, Table } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { useEffect, useState } from "react";
import AddDemand from "../components/forms/AddDemand";
import { useSelector } from "react-redux";
import { selectNotifications } from "../redux/selectors/notificationSelectors";

const INITIAL_VALUES = {
	title: "",
	description: "",
	notifyUserId: "",
	isModalVisible: false,
};

const URL = import.meta.env.VITE_API_BASE_URL + "api/Application/demands";

const DemandsPage = () => {
	const [api, contextHolder] = notification.useNotification();
	const [demands, setDemands] = useState([]);
	const [demand, setDemand] = useState(INITIAL_VALUES);
	const token = localStorage.getItem("token") || "";
	const notifications = useSelector(selectNotifications);

	const fetchDemands = async () => {
		try {
			if (!token) {
				api.error({
					message: "Token not found",
					description: "You need to login to access this page",
				});
				return;
			}

			const res = await fetch(URL, {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
				},
			});
			const data = await res.json();
			setDemands(data);
		} catch (error) {
			api.error({
				message: "Error",
				description: error.message,
			});
		}
	};

	useEffect(() => {
		fetchDemands();
	}, []);

	useEffect(() => {
		if (notifications.length) {
			api.open({
				message: "Notification",
				description: notifications[notifications.length - 1].message,
				placement: "bottomRight",
			});
		}
	}, [notifications, api]);

	const openModal = () => {
		setDemand({ ...INITIAL_VALUES, isModalVisible: true });
	};

	const closeModal = (isFetch) => {
		if (isFetch) {
			fetchDemands();
		}
		setDemand({ ...INITIAL_VALUES, isModalVisible: false });
	};

	const columns = [
		{
			title: "Title",
			dataIndex: "title",
			key: "title",
		},
		{
			title: "Description",
			dataIndex: "description",
			key: "description",
		},
	];

	return (
		<div className="grid p-8 gap-2.5">
			{contextHolder}
			<Row justify={"space-between"}>
				<Col>
					<h3 className="text-4xl font-black">Demands</h3>
				</Col>
				<Col>
					<Button
						className="btnGreen"
						type="button"
						icon={<PlusOutlined />}
						onClick={() => openModal()}
					>
						Add Demand
					</Button>
				</Col>
			</Row>
			{demand.isModalVisible && (
				<AddDemand
					initialValues={demand}
					isModalVisible={demand.isModalVisible}
					closeModal={closeModal}
					token={token}
				/>
			)}
			<Table
				dataSource={demands}
				columns={columns}
				rowKey={(record) => record.id}
			/>
		</div>
	);
};

export default DemandsPage;
