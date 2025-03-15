import { Button, Layout, theme } from "antd";
import { useDispatch, useSelector } from "react-redux";
import { Outlet, useNavigate } from "react-router-dom";
import { logout } from "../redux/slices/authSlice";
import { NotificationOutlined } from "@ant-design/icons";
import Notifications from "../components/Notification/Notifications";
import { useState } from "react";
import {
	selectPoke,
	selectStatus,
} from "../redux/selectors/notificationSelectors";
import { resetPoke } from "../redux/slices/notificationSlice";

const { Header, Content } = Layout;

const Dashboard = () => {
	const {
		token: { colorBgContainer, borderRadiusLG },
	} = theme.useToken();
	const userName = useSelector((state) => state.auth.userName);
	const connectionStatus = useSelector(selectStatus);
	const dispatch = useDispatch();
	const navigate = useNavigate();
	const [openNotification, setOpenNotification] = useState(false);
	const poke = useSelector(selectPoke);

	return (
		<Layout className="!min-h-screen">
			<Header style={{ padding: "0 20px", background: colorBgContainer }}>
				<div className="flex justify-between items-center h-full">
					<h1 className="text-2xl font-bold">Dashboard</h1>

					<div className="flex items-center space-x-2">
						<Button
							type="primary"
							onClick={() => {
								setOpenNotification(!openNotification);
								dispatch(resetPoke());
							}}
						>
							<NotificationOutlined />
							{poke && (
								<span className="absolute right-0 top-0 h-2 w-2 rounded-full bg-red-400"></span>
							)}
						</Button>

						{/* Notification component */}
						{openNotification && <Notifications />}

						<span>{connectionStatus}</span>

						<Button
							type="primary"
							onClick={() => {
								dispatch(logout());
								navigate("/login");
							}}
						>
							{userName} - Logout
						</Button>
					</div>
				</div>
			</Header>
			<Content
				style={{
					margin: "24px 16px",
					padding: 24,
					minHeight: 280,
					background: colorBgContainer,
					borderRadius: borderRadiusLG,
				}}
			>
				<Outlet />
			</Content>
		</Layout>
	);
};

export default Dashboard;
