// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Repositories;

namespace Domain.Queries.LoadUserSettings;

/// <summary>
/// Load a user's settings
/// </summary>
internal sealed class LoadUserSettingsHandler : QueryHandler<LoadUserSettingsQuery, UserSettings>
{
	private ILog<LoadUserSettingsHandler> Log { get; init; }

	private IUserSettingsRepository UserSettings { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="log"></param>
	/// <param name="userSettings"></param>
	public LoadUserSettingsHandler(ILog<LoadUserSettingsHandler> log, IUserSettingsRepository userSettings) =>
		(Log, UserSettings) = (log, userSettings);

	/// <summary>
	/// Retrieve the settings for user specified in <paramref name="query"/> - if none have been
	/// saved yet, returns a default object
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<UserSettings>> HandleAsync(LoadUserSettingsQuery query)
	{
		Log.Vrb("Load settings for User {UserId}", query.Id.Value);
		return UserSettings
			.StartFluentQuery()
			.Where(s => s.UserId, Compare.Equal, query.Id)
			.QuerySingleAsync<UserSettings>()
			.SwitchAsync(
				some: x => F.Some(x).AsTask(),
				none: _ => F.Some(new UserSettings()).AsTask()
			);
	}
}
