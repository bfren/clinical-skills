// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.Types.StrongIds;
using Jeebs.Data;
using Jeebs.Logging;

namespace ClinicalSkills.Persistence.Repositories;

/// <inheritdoc cref="IEntryThemeRepository"/>
public sealed class EntryThemeRepository : Repository<EntryThemeEntity, EntryThemeId>, IEntryThemeRepository
{
	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="db"></param>
	/// <param name="log"></param>
	public EntryThemeRepository(IDb db, ILog<EntryThemeRepository> log) : base(db, log) { }
}
