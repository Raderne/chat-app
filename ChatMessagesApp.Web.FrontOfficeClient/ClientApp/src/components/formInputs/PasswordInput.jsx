import { Input } from "antd";
import { getIn, useFormikContext } from "formik";
import { EyeInvisibleOutlined, EyeTwoTone } from "@ant-design/icons";

const PasswordInput = (props) => {
	const { name, prefix, placeholder, title, disabled = false } = props;
	const { values, handleBlur, errors, touched, handleChange } =
		useFormikContext();
	const value = getIn(values, name);
	const isTouched = getIn(touched, name);
	const error = getIn(errors, name);

	return (
		<div>
			{title && (
				<label
					htmlFor={name}
					className="mb-2 text-sm"
				>
					{title}
				</label>
			)}
			<Input.Password
				prefix={prefix || null}
				id={name}
				name={name}
				placeholder={placeholder}
				disabled={disabled}
				value={value}
				status={isTouched && error ? "error" : ""}
				help={isTouched && error ? error : ""}
				onBlur={handleBlur}
				onChange={handleChange}
				iconRender={(visible) =>
					visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />
				}
			/>
			{isTouched && error && <span className="text-red-500">{error}</span>}
		</div>
	);
};

export default PasswordInput;
