// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.GetClinicalSettings;
using Persistence.StrongIds;

namespace WebApp.Pages.Components.List;

public sealed class ClinicalSettingListViewComponent : ListSingleViewComponent<ClinicalSettingsModel, ClinicalSettingId>
{
	public ClinicalSettingListViewComponent() : base("clinical setting", x => x.Name, x => x.Name) { }
}
