// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Domain.GetUserProfile;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Jeebs.Mvc;
using Jeebs.Mvc.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicalSkills.WebApp.Pages.Auth;

[Authorize]
[ValidateAntiForgeryToken]
public sealed partial class ProfileModel : PageModel
{
	public UserProfileModel Profile { get; set; } = new();

	private IDispatcher Dispatcher { get; }

	private ILog<ProfileModel> Log { get; }

	public ProfileModel(IDispatcher dispatcher, ILog<ProfileModel> log) =>
		(Dispatcher, Log) = (dispatcher, log);

	public async Task<IActionResult> OnGetAsync()
	{
		var query = from u in User.GetUserId()
					from p in Dispatcher.DispatchAsync(new Domain.GetUserProfileQuery(u))
					select p;

		await foreach (var profile in query)
		{
			Profile = profile;
			return Page();
		}

		return RedirectToPage("./SignOut");
	}

	public async Task<IActionResult> OnGetInsertTestDataAsync()
	{
		var query = from r in Dispatcher.DispatchAsync(new Domain.InsertTestDataCommand())
					select r;

		return await query
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: x => Result.Create(x),
				none: r => Result.Error(r)
			);
	}
}
