import { Button } from "antd";
import TextInput from "../formInputs/TextInput";
import { FormikProvider, useFormik } from "formik";
import * as Yup from "yup";
import { getInitialsCharFromUsername } from "../../utils/getInitialsCharFromFullName";
import { useSignalR } from "../../context/SignalRContext";

const SendMessage = ({ demandId, sendToId }) => {
	const username = localStorage.getItem("userName");
	const { connection, safeInvoke } = useSignalR();

	const initialValues = {
		demandId,
		sendToId,
		message: "",
	};

	const validationSchema = Yup.object({
		message: Yup.string().required("Message is required"),
	});

	const handleSubmit = async (values) => {
		try {
			console.log(values.demandId);
			if (!connection) return;
			await safeInvoke("SendMessage");
			// formik.resetForm();
		} catch (error) {
			console.error(error);
		}
	};

	const formik = useFormik({
		initialValues,
		validationSchema,
		onSubmit: handleSubmit,
	});

	return (
		<FormikProvider value={formik}>
			<div className="flex gap-[20px]">
				<div className="!w-[40px] !h-[40px] rounded-full bg-[#164998] text-white flex items-center justify-center font-montserrat text-sm flex-shrink-0">
					{getInitialsCharFromUsername(username)}
				</div>
				<form
					className="flex-grow p-[2px]"
					onSubmit={formik.handleSubmit}
				>
					<TextInput
						name="userName"
						className="w-[100%] rounded-[4px]"
						placeholder="Ecrire un message"
						{...formik.getFieldProps("message")}
					/>
					<Button
						type="primary"
						className="custom-btn-radius-15 mt-[10px]"
						htmlType="submit"
					>
						envoyer
					</Button>
				</form>
			</div>
		</FormikProvider>
	);
};

export default SendMessage;
