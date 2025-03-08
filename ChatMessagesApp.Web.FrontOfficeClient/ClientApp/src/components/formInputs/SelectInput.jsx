import { Select } from "antd";
import { getIn, useFormikContext } from "formik";

const SelectInput = (props) => {
	const {
		name,
		prefix = null,
		placeholder,
		title,
		options = [],
		size = "default size",
		disabled = false,
		handleChange = () => {},
	} = props;
	const { values, setFieldValue, handleBlur, errors, touched } =
		useFormikContext();
	const error = getIn(errors, name);
	const isTouched = getIn(touched, name);
	const value = getIn(values, name);

	return (
		<>
			{title && (
				<label
					htmlFor={name}
					className="mb-2 text-sm"
				>
					{title}
				</label>
			)}
			<Select
				id={name}
				name={name}
				placeholder={placeholder}
				value={value}
				optionFilterProp="label"
				size={size}
				style={{ width: "100%" }}
				showSearch
				onChange={(value) => {
					setFieldValue(name, value);
					handleChange(value);
				}}
				onBlur={handleBlur}
				options={options}
				status={error && isTouched ? "error" : ""}
				prefix={prefix ? prefix : null}
				disabled={disabled}
			/>
			{error && isTouched && <span style={{ color: "red" }}>{error}</span>}
		</>
	);
};

export default SelectInput;
