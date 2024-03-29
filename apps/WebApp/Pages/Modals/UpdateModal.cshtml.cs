// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace WebApp.Pages.Modals;

public abstract class UpdateModalModel : ModalModel
{
	public string? AddItemUrl { get; set; }

	protected UpdateModalModel(string title) : base(title) { }

	protected UpdateModalModel(string title, string size) : base(title, size) { }
}
