import { useSelector } from "react-redux";

const ConnectionStatus = () => {
	const { status: connectionStatus } = useSelector(
		(state) => state.notification,
	);

	return <div>{connectionStatus.toUpperCase()}</div>;
};

export default ConnectionStatus;
