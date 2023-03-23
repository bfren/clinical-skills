// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Persistence.StrongIds;

namespace Domain.Commands.DeleteClinicalSetting;

/// <summary>
/// Used to return a clinical setting that is ready to be deleted
/// </summary>
/// <param name="Id">Clinical Setting ID</param>
/// <param name="Version">Concurrency version</param>
/// <param name="IsDisabled">Whether or not the Clinical Setting should be disabled</param>
internal sealed record class ClinicalSettingToDeleteModel(ClinicalSettingId Id, long Version, bool IsDisabled) : IWithVersion<ClinicalSettingId>
{
	public ClinicalSettingToDeleteModel() : this(new(), 0L, false) { }
}
