import { Button, Layout, theme } from "antd";
import { useDispatch, useSelector } from "react-redux";
import { Outlet, useNavigate } from "react-router-dom";
import { logout } from "../redux/slices/authSlice";
import { NotificationOutlined } from "@ant-design/icons";
import Notifications from "../components/Notification/Notifications";
import { useState } from "react";

const { Header, Content } = Layout;

const Dashboard = () => {
	const {
		token: { colorBgContainer, borderRadiusLG },
	} = theme.useToken();
	const userName = useSelector((state) => state.auth.userName);
	const dispatch = useDispatch();
	const navigate = useNavigate();
	const [openNotification, setOpenNotification] = useState(false);

	return (
		<Layout className="!min-h-screen">
			<Header style={{ padding: "0 20px", background: colorBgContainer }}>
				<div className="flex justify-between items-center h-full">
					<h1 className="text-2xl font-bold">Dashboard</h1>

					<div className="flex items-center space-x-2">
						{/* notification icon that opens Notification component */}
						<Button
							type="primary"
							onClick={() => {
								setOpenNotification(!openNotification);
							}}
						>
							<NotificationOutlined />
						</Button>

						{/* Notification component */}
						{openNotification && <Notifications />}

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
