// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.AspNetCore.Mvc;

namespace ClinicalSkills.WebApp.Pages.Components.Sidebar;

public sealed class SidebarViewComponent : ViewComponent
{
	public IViewComponentResult Invoke() =>
		View();
}
