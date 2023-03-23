// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveSkill.Internals;

/// <summary>
/// Create a new skill entity
/// </summary>
internal sealed class CreateSkillHandler : QueryHandler<CreateSkillQuery, SkillId>
{
	private ILog<CreateSkillHandler> Log { get; init; }

	private ISkillRepository Skill { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="log"></param>
	/// <param name="skill"></param>
	public CreateSkillHandler(ILog<CreateSkillHandler> log, ISkillRepository skill) =>
		(Log, Skill) = (log, skill);

	/// <summary>
	/// Create a new skill from <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<SkillId>> HandleAsync(CreateSkillQuery query)
	{
		Log.Vrb("Creating Skill: {Query}", query);
		return Skill
			.CreateAsync(new()
			{
				UserId = query.UserId,
				Name = query.Name
			});
	}
}
