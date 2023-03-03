/**
 * Select search box when search icon is clicked.
 *
 */
const searchForm = document.getElementById("search");
searchForm.addEventListener("shown.bs.collapse", _ => {
	searchForm.querySelector("input").select();
});
