// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTheme.Internals;

/// <summary>
/// Create a new theme entity
/// </summary>
internal sealed class CreateThemeHandler : QueryHandler<CreateThemeQuery, ThemeId>
{
	private ILog<CreateThemeHandler> Log { get; init; }

	private IThemeRepository Theme { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="log"></param>
	/// <param name="theme"></param>
	public CreateThemeHandler(ILog<CreateThemeHandler> log, IThemeRepository theme) =>
		(Log, Theme) = (log, theme);

	/// <summary>
	/// Create a new theme from <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<ThemeId>> HandleAsync(CreateThemeQuery query)
	{
		Log.Vrb("Creating Theme: {Query}", query);
		return Theme
			.CreateAsync(new()
			{
				UserId = query.UserId,
				Name = query.Name
			});
	}
}
