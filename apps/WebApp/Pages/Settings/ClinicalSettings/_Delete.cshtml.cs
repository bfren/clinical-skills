// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.CheckClinicalSettingCanBeDeleted.Messages;
using Domain.Queries.GetClinicalSetting;
using Jeebs.Mvc;
using Jeebs.Mvc.Auth;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.StrongIds;
using WebApp.Pages.Modals;

namespace WebApp.Pages.Settings.ClinicalSettings;

public sealed class DeleteModel : DeleteModalModel
{
	public ClinicalSettingModel ClinicalSetting { get; set; } = new();

	public DeleteModel() : base("ClinicalSetting") { }
}

public sealed partial class IndexModel
{
	public Task<PartialViewResult> OnGetDeleteAsync(ClinicalSettingId clinicalSettingId)
	{
		// Create query
		var query = from userId in User.GetUserId()
					from op in Dispatcher.SendAsync(new Q.CheckClinicalSettingCanBeDeletedQuery(userId, clinicalSettingId))
					from clinicalSetting in Dispatcher.SendAsync(new Q.GetClinicalSettingQuery(userId, clinicalSettingId))
					select new { op, clinicalSetting };

		return query
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: x => Partial("_Delete", new DeleteModel { ClinicalSetting = x.clinicalSetting, Operation = x.op }),
				none: r => r switch
				{
					ClinicalSettingIsDefaultClinicalSettingMsg =>
						Partial("_Delete", new DeleteModel
						{
							Operation = DeleteOperation.None,
							Reason = "it is the default clinical setting for new entries"
						}),

					_ =>
						Partial("Modals/ErrorModal", r)
				}
			);
	}

	public Task<IActionResult> OnPostDeleteAsync(C.DeleteClinicalSettingCommand deleteCommand)
	{
		var query = from u in User.GetUserId()
					from r in Dispatcher.SendAsync(deleteCommand with { UserId = u })
					select r;

		return query
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: async x => x switch
				{
					true =>
						await OnGetAsync(),

					false =>
						Result.Error("Unable to delete clinical setting.")
				},
				none: r => Result.Error(r)
			);
	}
}
