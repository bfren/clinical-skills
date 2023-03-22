// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Apps.Web.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.Components.Header;

public sealed class Menu
{
	public readonly record struct Item(string Text, string Page);

	public IList<Item> Items { get; init; } = new List<Item>();

	public Menu() =>
		Items = new List<Item>
		{
			{ new("Entries", "/Entries/Index") },
			{ new("Profile", "/Auth/Profile") },
			{ new("Sign Out", "/Auth/Signout") }
		};
}

public sealed record class HeaderModel(Menu Menu);

#if DEBUG
[ResponseCache(CacheProfileName = CacheProfiles.None)]
#else
[ResponseCache(CacheProfileName = CacheProfiles.Default)]
#endif
public sealed class HeaderViewComponent : ViewComponent
{
	public IViewComponentResult Invoke() =>
		View(new HeaderModel(new()));
}
