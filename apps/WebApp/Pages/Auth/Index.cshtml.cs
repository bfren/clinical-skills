// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Logging;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.Auth;

public sealed class IndexModel : Jeebs.Mvc.Razor.Pages.Auth.IndexModel
{
	public IndexModel(ILog<IndexModel> log) : base(log) { }

	public override IActionResult OnGet() =>
		RedirectToPage("Profile");
}
