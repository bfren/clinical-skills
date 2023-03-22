// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.StrongIds;

namespace Persistence.Repositories;

/// <inheritdoc cref="IThemeRepository"/>
public sealed class ThemeRepository : Repository<ThemeEntity, ThemeId>, IThemeRepository
{
	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="db"></param>
	/// <param name="log"></param>
	public ThemeRepository(IDb db, ILog<ThemeRepository> log) : base(db, log) { }
}
