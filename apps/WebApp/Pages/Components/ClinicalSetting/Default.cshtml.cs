// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;
using Jeebs.Logging;
using Jeebs.Mvc.Auth;
using Microsoft.AspNetCore.Mvc;
using Persistence.StrongIds;

namespace WebApp.Pages.Components.ClinicalSetting;

public sealed record class ClinicalSettingModel(string Label, string? UpdateUrl, ClinicalSettingId Id, string Name, EntryId? EntryId)
{
	public static ClinicalSettingModel Blank(string label, string? updateUrl) =>
		new(label, updateUrl, new(), string.Empty, null);
}

public sealed class ClinicalSettingViewComponent : ViewComponent
{
	private IDispatcher Dispatcher { get; }

	private ILog Log { get; }

	public ClinicalSettingViewComponent(IDispatcher dispatcher, ILog<ClinicalSettingViewComponent> log) =>
		(Dispatcher, Log) = (dispatcher, log);

	public async Task<IViewComponentResult> InvokeAsync(string label, string updateUrl, ClinicalSettingId value, EntryId? entryId)
	{
		if (value is null)
		{
			return View(ClinicalSettingModel.Blank(label, updateUrl));
		}

		Log.Dbg("Get clinical setting: {ClinicalSettingId}.", value);
		return await UserClaimsPrincipal
			.GetUserId()
			.BindAsync(x => Dispatcher.SendAsync(new Q.GetClinicalSettingQuery(x, value)))
			.AuditAsync(none: r => Log.Err("Unable to get clinical setting: {Reason}", r))
			.SwitchAsync(
				some: x => View(new ClinicalSettingModel(label, updateUrl, x.Id, x.Name, entryId)),
				none: _ => (IViewComponentResult)View(ClinicalSettingModel.Blank(label, updateUrl))
			);
	}
}
