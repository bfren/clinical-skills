// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Mvc.Auth;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.Entries;

public sealed record class MenuItem(
	string Text,
	string Page
);

public sealed record class Menu(
	string Title,
	MenuItem[] Items
);

public sealed record class SidebarModel(
	Menu[] Menus
);

public sealed partial class IndexModel
{
	public async Task<PartialViewResult> OnGetSidebarAsync()
	{
		if (User.GetUserId().IsSome(out var user))
		{
			var mainMenu = new Menu(
				Title: "Menu",
				Items: new MenuItem[]
				{
					new("Entries", "/Entries/Index"),
					new("Settings", "/Settings/Index")
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

			return Partial("_Sidebar", new SidebarModel(new[] { mainMenu, accountMenu }));
		}

		return Partial("_Sidebar", new SidebarModel(Array.Empty<Menu>()));
	}
}
