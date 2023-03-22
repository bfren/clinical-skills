// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Cqrs;
using Jeebs.Data;

namespace ClinicalSkills.Domain.SaveClinicalSetting.Internals;

/// <inheritdoc cref="UpdateClinicalSettingHandler"/>
/// <param name="Id">Clinical Setting ID</param>
/// <param name="Version">Entity Version</param>
/// <param name="Description">Description</param>
internal sealed record class UpdateClinicalSettingCommand(
	ClinicalSettingId Id,
	long Version,
	string Description
) : Command, IWithVersion<ClinicalSettingId>
{
	/// <summary>
	/// Create from a <see cref="SaveClinicalSettingQuery"/>
	/// </summary>
	/// <param name="clinicalSettingId"></param>
	/// <param name="query"></param>
	public UpdateClinicalSettingCommand(ClinicalSettingId clinicalSettingId, SaveClinicalSettingQuery query) : this(
		Id: clinicalSettingId,
		Version: query.Version,
		Description: query.Description
	)
	{ }
}
