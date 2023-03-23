// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.GetClinicalSettings;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Jeebs.Mvc;
using Jeebs.Mvc.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Settings.ClinicalSettings;

[Authorize]
[ValidateAntiForgeryToken]
public sealed partial class IndexModel : PageModel
{
	private IDispatcher Dispatcher { get; }

	private ILog<IndexModel> Log { get; }

	public List<ClinicalSettingsModel> ClinicalSettings { get; set; } = new();

	public IndexModel(IDispatcher dispatcher, ILog<IndexModel> log) =>
		(Dispatcher, Log) = (dispatcher, log);

	public async Task<IActionResult> OnGetAsync()
	{
		var query = from u in User.GetUserId()
					from c in Dispatcher.SendAsync(new Q.GetClinicalSettingsQuery(u, true))
					select c;

		await foreach (var clinicalSettings in query)
		{
			ClinicalSettings = clinicalSettings.ToList();
		}

		return Page();
	}

	public Task<IActionResult> OnPostAsync(Q.SaveClinicalSettingQuery clinicalSetting)
	{
		var query = from u in User.GetUserId()
					from r in Dispatcher.SendAsync(clinicalSetting with { UserId = u })
					select r;

		return query
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: _ => OnGetAsync(),
				none: r => Result.Error(r)
			);
	}
}
