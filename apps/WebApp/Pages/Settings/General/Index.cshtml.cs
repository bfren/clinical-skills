// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Jeebs.Mvc;
using Jeebs.Mvc.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Pages.Modals;

namespace WebApp.Pages.Settings.General;

public abstract class EditSettingsModel : UpdateModalModel
{
	public UserSettings Settings { get; set; } = new();

	protected EditSettingsModel(string title) : base(title) { }
}

[Authorize]
[ValidateAntiForgeryToken]
public sealed partial class IndexModel : PageModel
{
	private IDispatcher Dispatcher { get; }

	private ILog<IndexModel> Log { get; }

	public UserSettings Settings { get; set; } = new();

	public IndexModel(IDispatcher dispatcher, ILog<IndexModel> log) =>
		(Dispatcher, Log) = (dispatcher, log);

	public async Task<IActionResult> OnGetAsync()
	{
		var query = from u in User.GetUserId()
					from s in Dispatcher.SendAsync(new Q.LoadUserSettingsQuery(u))
					select s;

		await foreach (var settings in query)
		{
			Settings = settings;
		}

		return Page();
	}

	private Task<PartialViewResult> GetFieldAsync<TValue, TModel>(
		string partial,
		Func<AuthUserId, Task<Maybe<TValue>>> getValue,
		Func<UserSettings, TValue, TModel> getModel
	) where TModel : EditSettingsModel
	{
		// Build query
		var query = from u in User.GetUserId()
					from settings in Dispatcher.SendAsync(new Q.LoadUserSettingsQuery(u))
					from value in getValue(u)
					select new { settings, value };

		// Execute query and return partial
		return query
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: x => Partial("_Edit" + partial, getModel(x.settings, x.value)),
				none: r => Partial("Modals/ErrorModal", r)
			);
	}

	private Task<IActionResult> PostFieldAsync<TCommand, TValue>(
		string component,
		string label,
		TCommand command,
		Func<TCommand, TValue> getValue
	)
		where TCommand : Command, IWithUserId
	{
		// Get values
		var updateUrl = Url.Page("Index", "Edit" + component);
		var value = getValue(command);

		// Log operation
		Log.Vrb("Saving {Setting} for {User}.", component, User.GetUserId());

		// Build query
		var query = from userId in User.GetUserId()
					from result in Dispatcher.SendAsync(command with { UserId = userId })
					select result;

		return query
			.AuditAsync(none: Log.Msg)
			.SwitchAsync<bool, IActionResult>(
				some: x => x switch
				{
					true =>
						ViewComponent(component, new { label, updateUrl, value }),

					false =>
						Result.Error($"Unable to save {label}.")
				},
				none: r => Result.Error(r)
			);
	}
}
