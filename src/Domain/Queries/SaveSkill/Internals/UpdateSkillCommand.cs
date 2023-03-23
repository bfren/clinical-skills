// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;
using Jeebs.Data;
using Persistence.StrongIds;

namespace Domain.Queries.SaveSkill.Internals;

/// <inheritdoc cref="UpdateSkillHandler"/>
/// <param name="Id">Skill ID</param>
/// <param name="Version">Entity Version</param>
/// <param name="Name">Name</param>
internal sealed record class UpdateSkillCommand(
	SkillId Id,
	long Version,
	string Name
) : Command, IWithVersion<SkillId>
{
	/// <summary>
	/// Create from a <see cref="SaveSkillQuery"/>
	/// </summary>
	/// <param name="trainingGradeId"></param>
	/// <param name="query"></param>
	public UpdateSkillCommand(SkillId trainingGradeId, SaveSkillQuery query) : this(
		Id: trainingGradeId,
		Version: query.Version,
		Name: query.Name
	)
	{ }
}
