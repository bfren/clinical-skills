// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain.Queries.SaveSkill.Internals;

/// <inheritdoc cref="CreateSkillHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Name">Name</param>
internal sealed record class CreateSkillQuery(
	AuthUserId UserId,
	string Name
) : Query<SkillId>, IWithUserId
{
	/// <summary>
	/// Create from <see cref="SaveSkillQuery"/>
	/// </summary>
	/// <param name="query"></param>
	public CreateSkillQuery(SaveSkillQuery query) : this(
		UserId: query.UserId,
		Name: query.Name
	)
	{ }
}
