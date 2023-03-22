// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace ClinicalSkills.Domain.SaveClinicalSetting.Internals;

/// <inheritdoc cref="CreateClinicalSettingHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Description">Description</param>
internal sealed record class CreateClinicalSettingQuery(
	AuthUserId UserId,
	string Description
) : Query<ClinicalSettingId>, IWithUserId
{
	/// <summary>
	/// Create from <see cref="SaveClinicalSettingQuery"/>
	/// </summary>
	/// <param name="query"></param>
	public CreateClinicalSettingQuery(SaveClinicalSettingQuery query) : this(
		UserId: query.UserId,
		Description: query.Description
	)
	{ }
}
