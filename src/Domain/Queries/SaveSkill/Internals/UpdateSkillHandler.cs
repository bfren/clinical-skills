// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using MaybeF.Caching;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveSkill.Internals;

/// <summary>
/// Update an existing skill entity
/// </summary>
internal sealed class UpdateSkillHandler : CommandHandler<UpdateSkillCommand>
{
	private IMaybeCache<SkillId> Cache { get; init; }

	private ILog<UpdateSkillHandler> Log { get; init; }

	private ISkillRepository Skill { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="cache"></param>
	/// <param name="log"></param>
	/// <param name="skill"></param>
	public UpdateSkillHandler(IMaybeCache<SkillId> cache, ILog<UpdateSkillHandler> log, ISkillRepository skill) =>
		(Cache, Log, Skill) = (cache, log, skill);

	/// <summary>
	/// Update a skill from <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override Task<Maybe<bool>> HandleAsync(UpdateSkillCommand command)
	{
		Log.Vrb("Updating Skill: {Command}", command);
		return Skill
			.UpdateAsync(command)
			.IfSomeAsync(x => { if (x) { Cache.RemoveValue(command.Id); } });
	}
}
