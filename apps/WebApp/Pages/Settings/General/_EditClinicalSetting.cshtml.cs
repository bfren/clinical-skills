// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Commands;
using Domain.Queries.GetClinicalSettings;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.Settings.General;

public sealed class EditClinicalSettingModel : EditSettingsModel
{
	public List<ClinicalSettingsModel> ClinicalSettings { get; set; } = new();

	public EditClinicalSettingModel() : base("Default Clinical Setting") { }
}

public sealed partial class IndexModel
{
	public Task<PartialViewResult> OnGetEditClinicalSettingAsync() =>
		GetFieldAsync("ClinicalSetting",
			x => Dispatcher.SendAsync(new Q.GetClinicalSettingsQuery(x, false)),
			(s, v) => new EditClinicalSettingModel { Settings = s, ClinicalSettings = v.ToList() }
		);

	public Task<IActionResult> OnPostEditClinicalSettingAsync(UpdateDefaultClinicalSettingCommand settings) =>
		PostFieldAsync("ClinicalSetting", "Default Clinical Setting", settings, x => x.DefaultClinicalSettingId);
}
