// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.Repositories;

namespace Domain.Queries.CheckThemeBelongsToUser;

/// <summary>
/// Check a theme belongs to a user
/// </summary>
internal sealed class CheckThemeBelongsToUserHandler : QueryHandler<CheckThemeBelongsToUserQuery, bool>
{
	private IThemeRepository Theme { get; init; }

	private ILog<CheckThemeBelongsToUserHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public CheckThemeBelongsToUserHandler(IThemeRepository trainingGrade, ILog<CheckThemeBelongsToUserHandler> log) =>
		(Theme, Log) = (trainingGrade, log);

	/// <summary>
	/// Returns true if the theme belongs to the user defined by <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<bool>> HandleAsync(CheckThemeBelongsToUserQuery query)
	{
		Log.Vrb("Checking theme {ThemeId} belongs to user {UserId}.", query.ThemeId.Value, query.UserId.Value);
		return Theme
			.StartFluentQuery()
			.Where(c => c.Id, Compare.Equal, query.ThemeId)
			.Where(c => c.UserId, Compare.Equal, query.UserId)
			.QuerySingleAsync<ThemeEntity>()
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: _ => F.True,
				none: _ => F.False
			);
	}
}
