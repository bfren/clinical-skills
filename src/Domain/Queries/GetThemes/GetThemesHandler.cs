// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Repositories;

namespace Domain.Queries.GetThemes;

/// <summary>
/// Get themes
/// </summary>
internal sealed class GetThemesHandler : QueryHandler<GetThemesQuery, IEnumerable<ThemesModel>>
{
	private IThemeRepository Theme { get; init; }

	private ILog<GetThemesHandler> Log { get; init; }

	/// <summary>
	/// Inject dependency
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public GetThemesHandler(IThemeRepository clinicalSetting, ILog<GetThemesHandler> log) =>
		(Theme, Log) = (clinicalSetting, log);

	/// <summary>
	/// Get themes for the specified user, sorted by name
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<IEnumerable<ThemesModel>>> HandleAsync(GetThemesQuery query)
	{
		if (query.UserId is null || query.UserId.Value == 0)
		{
			return F.None<IEnumerable<ThemesModel>, Messages.UserIdIsNullMsg>().AsTask();
		}

		Log.Vrb("Get Themes for {User}.", query.UserId);
		return Theme
			.StartFluentQuery()
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.Sort(x => x.Name, SortOrder.Ascending)
			.QueryAsync<ThemesModel>();
	}
}
