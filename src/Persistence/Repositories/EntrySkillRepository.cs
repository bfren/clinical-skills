// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.StrongIds;

namespace Persistence.Repositories;

/// <inheritdoc cref="IEntrySkillRepository"/>
public sealed class EntrySkillRepository : Repository<EntrySkillEntity, EntrySkillId>, IEntrySkillRepository
{
	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="db"></param>
	/// <param name="log"></param>
	public EntrySkillRepository(IDb db, ILog<EntrySkillRepository> log) : base(db, log) { }
}
