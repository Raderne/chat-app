export const getInitialsCharFromUsername = (username) => {
	if (!username) return "";

	return username[0].toUpperCase();
};
