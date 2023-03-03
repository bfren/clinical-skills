const alertIcons = {
	close: $("<i/>").addClass("fa-solid fa-xmark"),
	info: $("<i/>").addClass("fa-solid fa-circle-info"),
	success: $("<i/>").addClass("fa-solid fa-check"),
	warning: $("<i/>").addClass("fa-solid fa-triangle-exclamation"),
	error: $("<i/>").addClass("fa-solid fa-ban"),
	update: $("<i/>").addClass("fa-solid fa-circle-plus"),
	delete: $("<i/>").addClass("fa-solid fa-circle-minus"),
	complete: $("<i/>").addClass("fa-solid fa-circle-check"),
	save: $("<i/>").addClass("fa-solid fa-ban")
}

const alertTypes = {
	info: "info",
	success: "success",
	warning: "warning",
	error: "error"
}

const message = $(".statusbar .message");
const alertCountdown = 3;
var alertTimeout = 0;

/**
 * Returns true if there is currently a visible message.
 *
 */
function isAlert() {
	return message.is(":visible");
}

/**
 * Returns true if there is currently a visible message and it's sticky.
 *
 */
function isAlertSticky() {
	return isAlert() && message.data("sticky");
}

/**
 * Show an alert.
 * 
 * @param {string} type Alert type - see alertIcons for valid values
 * @param {string} text Alert text
 * @param {boolean} sticky If true the alert will not be automatically closed
 */
function showAlert(type, text, sticky) {
	if (type == alertTypes.info && isAlertSticky()) {
		return;
	}

	// set alert values
	message.find(".icon").html(alertIcons[type]);
	message.find(".text").text(text);
	message.find(".countdown").text("");
	message.data("type", type);
	message.data("sticky", sticky ? "true" : "false");

	// show alert and clear any existing timeouts
	message.stop().show();
	clearTimeout(alertTimeout);

	// make error alerts sticky
	if (type == alertTypes.error || sticky) {
		message.find(".close").show();
		return;
	}

	// start countdown to hide other alerts automatically
	message.find(".close").hide();
	updateAlert(alertCountdown);
}

/**
 * Update the current alert - remove automatically when seconds gets to 0.
 * 
 * @param {number} seconds
 */
function updateAlert(seconds) {
	if (seconds == 0) {
		closeAlert(true);
		return;
	}

	message.find(".countdown").text(seconds);
	alertTimeout = setTimeout(() => updateAlert(seconds - 1), 1000);
}

/**
 * Close the alert, if it is an info alert - or force is true.
 * 
 * @param {string} force If true the alert will be closed whatever type it is
 */
function closeAlert(force) {
	if (force || message.data("type") == alertTypes.info) {
		message.fadeOut();
	}
}
ready(() => message.click(
	() => closeAlert(true)
));

/**
 * Load the home page.
 * 
 */
function loadHome() {
	loadPage(home);
}

/**
 * Load a page into the main content block.
 * 
 * @param {any} url
 */
function loadPage(url) {
	if (getHash() == url) {
		loadHash();
	} else {
		window.location.hash = url;
	}
}

/**
 * Open whatever is in the URL hash.
 *
 */
function loadHash() {
	// get hash
	var url = getHash();
	if (!url || url.length == 2) {
		url = home;
	}

	// get URL contents
	setupAjaxAuth();
	$.ajax(
		{
			url: url,
			method: "GET"
		})

		.done(function (data, status, xhr) {
			// close info alert
			closeAlert();

			// handle JSON response
			if (xhr.responseJSON) {
				handleResult(xhr.responseJSON);
				return;
			}

			// replace HTML
			$("#content").html(data);

			// scroll to top of page
			window.scrollTo(0, 0);
		})

		.fail(function (xhr) {
			// close info alert
			closeAlert();

			// handle unauthorised
			if (xhr.status == 401) {
				setupAjaxAuth();
				$("#content").load(signIn);
				return;
			}

			// the response is a JSON result
			if (xhr && xhr.responseJSON) {
				handleResult(xhr.responseJSON);
				return;
			}

			// something else has gone wrong
			showAlert(alertTypes.error, "Something went wrong, please try again.");
		});;
}
ready(loadHash);
window.onhashchange = loadHash;

/**
 * Get the current window hash without the actual #.
 *
 */
function getHash() {
	return window.location.hash.replace("#", "");
}

/**
 * Capture link clicks and use AJAX to load pages.
 *
 */
function setupLinks() {
	$("body").on("click", "a", function (e) {
		// get hyperlink
		var href = $(this).attr("href");

		// ignore absolute links
		if (href.startsWith("http")) {
			return;
		}

		// ignore javascript links
		if (href == "javascript:void(0)") {
			return;
		}

		// stop default behaviour
		e.preventDefault();

		// load URL
		loadPage(href);
	})
}
ready(setupLinks);

/**
 * Handle a JSON Result object.
 * 
 * @param {any} r
 */
function handleResult(r) {
	// show alert
	var sticky = r.message.type == alertTypes.warning || r.message.type == alertTypes.error;
	showAlert(r.message.type, r.message.text, sticky);

	// if message is sign in, set JWT and load home page
	if (r.message.text == "You were signed in." && r.value) {
		setAuth(r.value);
		return loadPage(home);
	}

	// redirect if value is a URL	
	if (r.value) {
		if (r.value == "/") {
			return loadPage(home);
		} else if (r.value == "refresh") {
			return loadHash();
		} else if (v.value.startsWith("/")) {
			return loadPage(r.redirectTo);
		}
	}
}

/**
 * Select search box when search icon is clicked.
 *
 */
const searchForm = document.getElementById("search");
searchForm.addEventListener("shown.bs.collapse", _ => {
	searchForm.querySelector("input").select();
});

/**
 * Submit all forms via AJAX and handle results.
 *
 */
function setupAjaxSubmit() {
	$("body").on("submit", "form", function (e) {
		// stop default submit behaviour
		e.preventDefault();

		// check validity
		var form = $(this);
		if (this.checkValidity() === false) {
			form.find(":input:visible").not("[formnovalidate]")
				.parent().addClass("was-validated");
			return;
		}

		// support GET requests
		if (form.attr("method").toLowerCase() == "get") {
			var data = form.serialize();
			var url = form.attr("action") + "?" + data;
			return loadPage(url);
		}

		// submit form
		submitForm(form);
	});
}
ready(setupAjaxSubmit);

/**
 * Close a modal and submit a form, optionally overriding URL and data
 * 
 * @param {JQuery<any>} form Form to submit
 * @param {string} url Override form action and submit to this URL instead
 * @param {any} data Override form data and submit this data instead
 */
function submitForm(form, url, data) {
	// get form info
	var method = form.attr("method") ?? "POST";
	var replaceId = form.data("replace");
	var replaceContents = form.data("replace-contents");

	// hide modal
	if (modal) {
		modal.hide();
	}

	// post data and handle result
	setupAjaxAuth();
	$.ajax(
		{
			method: method,
			url: url || form.attr("action"),
			data: data || form.serialize()
		})

		.done(function (data, status, xhr) {
			// handle JSON response
			if (xhr.responseJSON) {
				handleResult(xhr.responseJSON);
				return;
			}

			// handle HTML response
			if (data && replaceId) {
				// get the DOM element to be replaced
				var replace = $("#" + replaceId);

				// replace contents or the element itself
				if (replaceContents) {
					replace.html(data);
				} else {
					replace.replaceWith(data);
				}

				// show alert
				if (method == "POST") {
					showAlert(alertTypes.success, "Done.");
				}

				return;
			}

			// something unexpected has happened
			showAlert(alertTypes.warning, "Something went wrong, refreshing the page.", true);
			loadHash();
		})

		.fail(function (xhr) {
			// the response is a JSON result
			if (xhr && xhr.responseJSON) {
				handleResult(xhr.responseJSON);
				return;
			}

			// something else has gone wrong
			showAlert(alertTypes.error, "Something went wrong, please try again.");
		});
}
