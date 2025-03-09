import { Button, Col, Modal, notification, Row } from "antd";
import { FormikProvider, useFormik } from "formik";
import { useEffect, useState } from "react";
import * as Yup from "yup";
import TextInput from "../formInputs/TextInput";
import SelectInput from "../formInputs/SelectInput";
import { useSelector } from "react-redux";
import { useSignalR } from "../../context/SignalRContext";

const URL = import.meta.env.VITE_API_BASE_URL;

const AddDemand = ({ initialValues, isModalVisible, closeModal, token }) => {
	const [users, setUsers] = useState([]);
	const { status: connectionStatus } = useSelector(
		(state) => state.notification,
	);
	const { connection, invoke } = useSignalR();

	const handleSubmitForm = async (values) => {
		try {
			const payload = {
				title: values.title,
				description: values.description,
				notifyUserId: values.notifyUserId,
			};

			const res = await fetch(URL + "api/Application/create-demand", {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
				body: JSON.stringify(payload),
			});

			if (!res.ok) {
				notification.error({
					message: "Error",
					description: "Failed to add demand",
				});
				return;
			}

			notification.success({
				message: "Success",
				description: "Demand added successfully",
			});

			if (connectionStatus === "connected" && connection) {
				await invoke(
					"NotifyUserAsync",
					"DemandCreated",
					values.notifyUserId,
					`New demand created: ${values.title} by ${
						users.find((user) => user.id === values.notifyUserId).firstName
					}`,
				);
			}

			handleCloseModal(true);
		} catch (error) {
			notification.error({
				message: "Error",
				description: error.message,
			});
		}
	};

	const validationSchema = Yup.object().shape({
		title: Yup.string().required("Title is required"),
		description: Yup.string().required("Description is required"),
		notifyUserId: Yup.string().required("Notify User is required"),
	});

	const formik = useFormik({
		initialValues,
		validationSchema,
		onSubmit: handleSubmitForm,
	});

	const fetchUsers = async () => {
		try {
			const res = await fetch(URL + "api/users", {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
			});
			const data = await res.json();
			setUsers(data);
		} catch (error) {
			notification.error({
				message: "Error",
				description: error.message,
			});
		}
	};

	useEffect(() => {
		fetchUsers();
	}, []);

	const handleCloseModal = (fetch = false) => {
		formik.resetForm();
		closeModal(fetch);
	};

	return (
		<>
			<FormikProvider value={formik}>
				<Modal
					width={700}
					title="Add Demand"
					open={isModalVisible}
					onCancel={() => handleCloseModal()}
					footer={null}
				>
					<form onSubmit={formik.handleSubmit}>
						<Row gutter={[16, 8]}>
							<Col span={12}>
								<TextInput
									name="title"
									title="Title"
									placeholder="Enter Title of Demand"
								/>
							</Col>
							<Col span={12}>
								<TextInput
									name="description"
									title="Description"
									placeholder="Enter Description of Demand"
								/>
							</Col>
							<Col span={12}>
								<SelectInput
									name="notifyUserId"
									title="Notify User"
									placeholder="Select Notify User"
									options={users.map((user) => ({
										value: user.id,
										label: user.firstName,
									}))}
								/>
							</Col>
						</Row>
						<Row
							justify="end"
							gutter={8}
							style={{ marginTop: "16px" }}
						>
							<Col>
								<Button
									type="default"
									onClick={() => handleCloseModal()}
								>
									Cancel
								</Button>
							</Col>
							<Col>
								<Button
									type="primary"
									htmlType="submit"
								>
									Add Demand
								</Button>
							</Col>
						</Row>
					</form>
				</Modal>
			</FormikProvider>
		</>
	);
};

export default AddDemand;
