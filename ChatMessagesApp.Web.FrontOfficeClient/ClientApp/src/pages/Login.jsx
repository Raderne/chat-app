import { Button } from "antd";
import { FormikProvider, useFormik } from "formik";
import * as Yup from "yup";
import TextInput from "../components/formInputs/TextInput";
import PasswordInput from "../components/formInputs/PasswordInput";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { setValues } from "../redux/slices/authSlice";

const URL = import.meta.env.VITE_API_BASE_URL + "api/users/login";

const Login = () => {
	const dispatch = useDispatch();
	const navigate = useNavigate();
	const auth = useSelector((state) => state.auth);

	const login = async (values) => {
		console.log(auth);
		try {
			const response = await fetch(
				import.meta.env.VITE_API_BASE_URL + "api/users/login",
				{
					method: "POST",
					headers: {
						"Content-Type": "application/json",
					},
					body: JSON.stringify(values),
				},
			);
			const data = await response.json();

			if (data?.token) {
				console.log(data);
				dispatch(setValues(data));
				navigate("/");
			} else {
				alert("Invalid email or password");
			}
		} catch (error) {
			console.error(error);
		}
	};

	const formik = useFormik({
		initialValues: {
			email: "",
			password: "",
		},
		validationSchema: Yup.object({
			email: Yup.string().email("Invalid email").required("Required"),
			password: Yup.string().required("Required"),
		}),
		onSubmit: (values) => {
			login(values);
		},
	});

	return (
		<FormikProvider value={formik}>
			<section className="min-h-screen flex justify-center items-center">
				<form
					onSubmit={formik.handleSubmit}
					className="bg-white p-8 rounded-lg shadow-md text-black space-y-4 w-96"
				>
					<h2 className="text-3xl text-center font-bold">Login</h2>
					<TextInput
						title="Email"
						name="email"
						placeholder="Email"
					/>
					<PasswordInput
						title="Password"
						name="password"
						placeholder="Password"
					/>
					<Button
						type="primary"
						htmlType="submit"
						className="w-full"
					>
						Login
					</Button>
				</form>
			</section>
		</FormikProvider>
	);
};

export default Login;
