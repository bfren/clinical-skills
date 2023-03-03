// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.AspNetCore.Mvc;

namespace ClinicalSkills.WebApp.Pages.Components.Sidebar;

public sealed record class SidebarModel(
	Menu[]? Menus
);

public class SidebarViewComponent : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		if (HttpContext.User.Identity?.IsAuthenticated == true)
		{
			var mainMenu = new Menu(
				Title: "Menu",
				Items: new MenuItem[]
				{
					new("Entries", "/Entries/Index")
				}
			);

			var accountMenu = new Menu(
				Title: "Account",
				Items: new MenuItem[]
				{
					new("Profile", "/Auth/Profile"),
					new("Sign Out", "/Auth/SignOut")
				}
			);

			return View(new SidebarModel(new[] { mainMenu, accountMenu }));
		}

		return View(new SidebarModel(null));
	}
}
