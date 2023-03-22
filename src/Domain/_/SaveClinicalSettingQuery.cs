// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.SaveClinicalSetting;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain;

/// <inheritdoc cref="SaveClinicalSettingHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Id">Clinical Setting ID</param>
/// <param name="Version">Entity Verion</param>
/// <param name="Name">Name</param>
public sealed record class SaveClinicalSettingQuery(
	AuthUserId UserId,
	ClinicalSettingId? Id,
	long Version,
	string Name
) : Query<ClinicalSettingId>, IWithUserId
{
	/// <summary>
	/// Create with minimum required values (for new clinical settings)
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="name"></param>
	public SaveClinicalSettingQuery(AuthUserId userId, string name) : this(
		UserId: userId,
		Id: null,
		Version: 0L,
		Name: name
	)
	{ }

	/// <summary>
	/// Create empty for model binding
	/// </summary>
	public SaveClinicalSettingQuery() : this(new(), string.Empty) { }
}
