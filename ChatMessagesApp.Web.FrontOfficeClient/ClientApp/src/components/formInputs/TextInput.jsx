import { Input } from "antd";
import { getIn, useFormikContext } from "formik";

const TextInput = (props) => {
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
			<Input
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
			/>
			{isTouched && error && <span className="text-red-500">{error}</span>}
		</div>
	);
};

export default TextInput;
