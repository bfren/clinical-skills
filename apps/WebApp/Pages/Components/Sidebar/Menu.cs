// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace ClinicalSkills.WebApp.Pages.Components.Sidebar;

public sealed record class Menu(
	string Title,
	MenuItem[] Items
);

public sealed record class MenuItem(
	string Text,
	string Page
);
