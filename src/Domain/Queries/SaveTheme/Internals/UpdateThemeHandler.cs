// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using MaybeF.Caching;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTheme.Internals;

/// <summary>
/// Update an existing theme entity
/// </summary>
internal sealed class UpdateThemeHandler : CommandHandler<UpdateThemeCommand>
{
	private IMaybeCache<ThemeId> Cache { get; init; }

	private ILog<UpdateThemeHandler> Log { get; init; }

	private IThemeRepository Theme { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="cache"></param>
	/// <param name="log"></param>
	/// <param name="theme"></param>
	public UpdateThemeHandler(IMaybeCache<ThemeId> cache, ILog<UpdateThemeHandler> log, IThemeRepository theme) =>
		(Cache, Log, Theme) = (cache, log, theme);

	/// <summary>
	/// Update a theme from <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override Task<Maybe<bool>> HandleAsync(UpdateThemeCommand command)
	{
		Log.Vrb("Updating Theme: {Command}", command);
		return Theme
			.UpdateAsync(command)
			.IfSomeAsync(x => { if (x) { Cache.RemoveValue(command.Id); } });
	}
}
