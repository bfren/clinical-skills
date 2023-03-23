// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.Repositories;

namespace Domain.Queries.CheckSkillBelongsToUser;

/// <summary>
/// Check a skill belongs to a user
/// </summary>
internal sealed class CheckSkillBelongsToUserHandler : QueryHandler<CheckSkillBelongsToUserQuery, bool>
{
	private ISkillRepository Skill { get; init; }

	private ILog<CheckSkillBelongsToUserHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public CheckSkillBelongsToUserHandler(ISkillRepository trainingGrade, ILog<CheckSkillBelongsToUserHandler> log) =>
		(Skill, Log) = (trainingGrade, log);

	/// <summary>
	/// Returns true if the skill belongs to the user defined by <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<bool>> HandleAsync(CheckSkillBelongsToUserQuery query)
	{
		Log.Vrb("Checking skill {SkillId} belongs to user {UserId}.", query.SkillId.Value, query.UserId.Value);
		return Skill
			.StartFluentQuery()
			.Where(c => c.Id, Compare.Equal, query.SkillId)
			.Where(c => c.UserId, Compare.Equal, query.UserId)
			.QuerySingleAsync<SkillEntity>()
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: _ => F.True,
				none: _ => F.False
			);
	}
}
