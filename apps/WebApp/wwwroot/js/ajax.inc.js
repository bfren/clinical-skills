const jwt_auth_token = "jwt";

/**
 * Set authorization token using local storage for persistence.
 *
 * @param {any} token Token value.
 */
function setAuth(token) {
	if (token) {
		localStorage.setItem(jwt_auth_token, "Bearer " + token);
		setupAjaxAuth();
	} else {
		localStorage.clear();
	}
}

/**
 * Add authorization header to the next AJAX request.
 *
 */
function setupAjaxAuth() {
	$.ajaxSetup({
		beforeSend: (xhr) => {
			xhr.setRequestHeader("Authorization", localStorage.getItem(jwt_auth_token))
		}
	});
}
