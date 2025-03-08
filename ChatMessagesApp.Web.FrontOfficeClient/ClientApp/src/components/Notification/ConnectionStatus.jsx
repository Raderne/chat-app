import { useSelector } from "react-redux";
import { selectConnectionStatus } from "../../redux/selectors/notificationSelectors";

const ConnectionStatus = () => {
	const status = useSelector(selectConnectionStatus);

	return <div>{status.toUpperCase()}</div>;
};

export default ConnectionStatus;
