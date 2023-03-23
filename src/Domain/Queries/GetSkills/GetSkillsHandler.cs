// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Repositories;

namespace Domain.Queries.GetSkills;

/// <summary>
/// Get skills
/// </summary>
internal sealed class GetSkillsHandler : QueryHandler<GetSkillsQuery, IEnumerable<SkillsModel>>
{
	private ISkillRepository Skill { get; init; }

	private ILog<GetSkillsHandler> Log { get; init; }

	/// <summary>
	/// Inject dependency
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public GetSkillsHandler(ISkillRepository clinicalSetting, ILog<GetSkillsHandler> log) =>
		(Skill, Log) = (clinicalSetting, log);

	/// <summary>
	/// Get skills for the specified user, sorted by name
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<IEnumerable<SkillsModel>>> HandleAsync(GetSkillsQuery query)
	{
		if (query.UserId is null || query.UserId.Value == 0)
		{
			return F.None<IEnumerable<SkillsModel>, Messages.UserIdIsNullMsg>().AsTask();
		}

		Log.Vrb("Get Skills for {User}.", query.UserId);
		return Skill
			.StartFluentQuery()
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.Sort(x => x.Name, SortOrder.Ascending)
			.QueryAsync<SkillsModel>();
	}
}
