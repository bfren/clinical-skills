// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTheme;

/// <summary>
/// Save a theme - create if it doesn't exist, or update if it does
/// </summary>
internal sealed class SaveThemeHandler : QueryHandler<SaveThemeQuery, ThemeId>
{
	private IDispatcher Dispatcher { get; init; }

	private ILog<SaveThemeHandler> Log { get; init; }

	private IThemeRepository Theme { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="dispatcher"></param>
	/// <param name="log"></param>
	/// <param name="theme"></param>
	public SaveThemeHandler(IDispatcher dispatcher, ILog<SaveThemeHandler> log, IThemeRepository theme) =>
		(Dispatcher, Log, Theme) = (dispatcher, log, theme);

	/// <summary>
	/// Save the theme belonging to user specified in <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override async Task<Maybe<ThemeId>> HandleAsync(SaveThemeQuery query)
	{
		Log.Vrb("Saving Theme {Query}.", query);

		// Ensure the theme belongs to the user
		if (query.Id?.Value > 0)
		{
			var themeBelongsToUser = await Dispatcher
				.SendAsync(new CheckThemeBelongsToUserQuery(query.UserId, query.Id))
				.IsTrueAsync();

			if (!themeBelongsToUser)
			{
				return F.None<ThemeId>(new Messages.ThemeDoesNotBelongToUserMsg(query.UserId, query.Id));
			}
		}

		// Create or update theme
		return await Theme
			.StartFluentQuery()
			.Where(x => x.Id, Compare.Equal, query.Id)
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.QuerySingleAsync<ThemeEntity>()
			.SwitchAsync(
				some: x => Dispatcher
					.SendAsync(new Internals.UpdateThemeCommand(x.Id, query))
					.BindAsync(_ => F.Some(x.Id)),
				none: () => Dispatcher
					.SendAsync(new Internals.CreateThemeQuery(query))
			);
	}
}
