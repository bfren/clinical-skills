// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveSkill;

/// <summary>
/// Save a skill - create if it doesn't exist, or update if it does
/// </summary>
internal sealed class SaveSkillHandler : QueryHandler<SaveSkillQuery, SkillId>
{
	private IDispatcher Dispatcher { get; init; }

	private ILog<SaveSkillHandler> Log { get; init; }

	private ISkillRepository Skill { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="dispatcher"></param>
	/// <param name="log"></param>
	/// <param name="skill"></param>
	public SaveSkillHandler(IDispatcher dispatcher, ILog<SaveSkillHandler> log, ISkillRepository skill) =>
		(Dispatcher, Log, Skill) = (dispatcher, log, skill);

	/// <summary>
	/// Save the skill belonging to user specified in <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override async Task<Maybe<SkillId>> HandleAsync(SaveSkillQuery query)
	{
		Log.Vrb("Saving Skill {Query}.", query);

		// Ensure the skill belongs to the user
		if (query.Id?.Value > 0)
		{
			var skillBelongsToUser = await Dispatcher
				.DispatchAsync(new CheckSkillBelongsToUserQuery(query.UserId, query.Id))
				.IsTrueAsync();

			if (!skillBelongsToUser)
			{
				return F.None<SkillId>(new Messages.SkillDoesNotBelongToUserMsg(query.UserId, query.Id));
			}
		}

		// Create or update skill
		return await Skill
			.StartFluentQuery()
			.Where(x => x.Id, Compare.Equal, query.Id)
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.QuerySingleAsync<SkillEntity>()
			.SwitchAsync(
				some: x => Dispatcher
					.DispatchAsync(new Internals.UpdateSkillCommand(x.Id, query))
					.BindAsync(_ => F.Some(x.Id)),
				none: () => Dispatcher
					.DispatchAsync(new Internals.CreateSkillQuery(query))
			);
	}
}
