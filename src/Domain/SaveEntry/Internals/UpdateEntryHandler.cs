// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using ClinicalSkills.Persistence.Repositories;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Cqrs;
using Jeebs.Logging;
using MaybeF.Caching;

namespace ClinicalSkills.Domain.SaveEntry.Internals;

/// <summary>
/// Update an existing entry entity
/// </summary>
internal sealed class UpdateEntryHandler : CommandHandler<UpdateEntryCommand>
{
	private IMaybeCache<EntryId> Cache { get; init; }

	private IEntryRepository Entry { get; init; }

	private ILog<UpdateEntryHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="cache"></param>
	/// <param name="entry"></param>
	/// <param name="log"></param>
	public UpdateEntryHandler(IMaybeCache<EntryId> cache, IEntryRepository entry, ILog<UpdateEntryHandler> log) =>
		(Cache, Entry, Log) = (cache, entry, log);

	/// <summary>
	/// Update a entry from <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override Task<Maybe<bool>> HandleAsync(UpdateEntryCommand command)
	{
		Log.Vrb("Updating Entry: {Command}", command);
		return Entry
			.UpdateAsync(command)
			.IfSomeAsync(x => { if (x) { Cache.RemoveValue(command.Id); } });
	}
}
