// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.AspNetCore.Mvc;

namespace ClinicalSkills.WebApp.Pages.Entries;

public sealed record class HomeModel;

public sealed partial class IndexModel
{
	public IActionResult OnGetHome() =>
		Partial("_Home", new HomeModel());
}
