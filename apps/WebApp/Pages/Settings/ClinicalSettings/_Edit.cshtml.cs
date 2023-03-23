// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.GetClinicalSetting;
using Jeebs.Mvc.Auth;
using Microsoft.AspNetCore.Mvc;
using Persistence.StrongIds;
using WebApp.Pages.Modals;

namespace WebApp.Pages.Settings.ClinicalSettings;

public sealed class EditModel : UpdateModalModel
{
	public ClinicalSettingModel ClinicalSetting { get; set; } = new();

	public EditModel() : base("ClinicalSetting") { }
}

public sealed partial class IndexModel
{
	public async Task<PartialViewResult> OnGetEditAsync(ClinicalSettingId clinicalSettingId)
	{
		// Create query
		var query = from u in User.GetUserId()
					from c in Dispatcher.SendAsync(new Q.GetClinicalSettingQuery(u, clinicalSettingId))
					select c;

		return await query
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: x => Partial("_Edit", new EditModel { ClinicalSetting = x }),
				none: r => Partial("Modals/ErrorModal", r)
			);
	}
}
