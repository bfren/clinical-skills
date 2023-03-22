// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain.SaveClinicalSetting.Internals;

/// <inheritdoc cref="CreateClinicalSettingHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Name">Name</param>
internal sealed record class CreateClinicalSettingQuery(
	AuthUserId UserId,
	string Name
) : Query<ClinicalSettingId>, IWithUserId
{
	/// <summary>
	/// Create from <see cref="SaveClinicalSettingQuery"/>
	/// </summary>
	/// <param name="query"></param>
	public CreateClinicalSettingQuery(SaveClinicalSettingQuery query) : this(
		UserId: query.UserId,
		Name: query.Name
	)
	{ }
}
