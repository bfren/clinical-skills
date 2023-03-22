// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.SaveSkill;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain;

/// <inheritdoc cref="SaveSkillHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Id">Skill ID</param>
/// <param name="Version">Entity Verion</param>
/// <param name="Name">Name</param>
public sealed record class SaveSkillQuery(
	AuthUserId UserId,
	SkillId? Id,
	long Version,
	string Name
) : Query<SkillId>, IWithUserId
{
	/// <summary>
	/// Create with minimum required values (for new skills)
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="name"></param>
	public SaveSkillQuery(AuthUserId userId, string name) : this(
		UserId: userId,
		Id: null,
		Version: 0L,
		Name: name
	)
	{ }

	/// <summary>
	/// Create empty for model binding
	/// </summary>
	public SaveSkillQuery() : this(new(), string.Empty) { }
}
