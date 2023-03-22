// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;
using Jeebs.Data;
using Persistence.StrongIds;

namespace Domain.SaveClinicalSetting.Internals;

/// <inheritdoc cref="UpdateClinicalSettingHandler"/>
/// <param name="Id">Clinical Setting ID</param>
/// <param name="Version">Entity Version</param>
/// <param name="Name">Name</param>
internal sealed record class UpdateClinicalSettingCommand(
	ClinicalSettingId Id,
	long Version,
	string Name
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
		Name: query.Name
	)
	{ }
}
