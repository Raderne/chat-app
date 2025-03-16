import { Button } from "antd";
import TextInput from "../formInputs/TextInput";
import { FormikProvider, useFormik } from "formik";
import * as Yup from "yup";
import { getInitialsCharFromUsername } from "../../utils/getInitialsCharFromFullName";

const URL = import.meta.env.VITE_API_BASE_URL + "api/Application/send-message";

const SendMessage = ({ demandId, sendTo, setMessageData }) => {
	const username = localStorage.getItem("userName");

	const initialValues = {
		demandId,
		sendToId: "",
		content: "",
	};

	const validationSchema = Yup.object({
		content: Yup.string().required("Message is required"),
	});

	const sendMessage = async (values) => {
		try {
			fetch(URL, {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
				},
				body: JSON.stringify(values),
			});
		} catch (error) {
			console.error(error);
		}
	};

	const handleSubmit = async (values) => {
		try {
			values.sendToId = sendTo;
			setMessageData((prev) => [
				...prev,
				{
					grade: "Moi",
					userName: username,
					message: values.content,
					publishDate: new Date().toLocaleDateString(),
				},
			]);
			await sendMessage(values);
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
						name="content"
						className="w-[100%] rounded-[4px]"
						placeholder="Ecrire un message"
						{...formik.getFieldProps("content")}
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
