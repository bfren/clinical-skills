// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.StrongIds;

namespace Persistence.Repositories;

/// <inheritdoc cref="IEntryRepository"/>
public sealed class EntryRepository : Repository<EntryEntity, EntryId>, IEntryRepository
{
	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="db"></param>
	/// <param name="log"></param>
	public EntryRepository(IDb db, ILog<EntryRepository> log) : base(db, log) { }
}
