// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Modals;

public abstract class ModalModel : PageModel
{
	public string? Size { get => size; set => size = $"modal-{value}"; }
	private string? size;

	public string Title { get; set; }

	protected ModalModel(string title) =>
		Title = title;

	protected ModalModel(string title, string size) : this(title) =>
		Size = size;
}
