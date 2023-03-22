// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.Repositories;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;

namespace ClinicalSkills.Domain.CheckEntryBelongsToUser;

/// <summary>
/// Check an entry belongs to a user
/// </summary>
internal sealed class CheckEntryBelongsToUserHandler : QueryHandler<CheckEntryBelongsToUserQuery, bool>
{
	private IEntryRepository Entry { get; init; }

	private ILog<CheckEntryBelongsToUserHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="entry"></param>
	/// <param name="log"></param>
	public CheckEntryBelongsToUserHandler(IEntryRepository entry, ILog<CheckEntryBelongsToUserHandler> log) =>
		(Entry, Log) = (entry, log);

	/// <summary>
	/// Returns true if the entry belongs to the user defined by <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<bool>> HandleAsync(CheckEntryBelongsToUserQuery query)
	{
		Log.Vrb("Checking entry {EntryId} belongs to user {UserId}.", query.EntryId.Value, query.UserId.Value);
		return Entry
			.StartFluentQuery()
			.Where(c => c.Id, Compare.Equal, query.EntryId)
			.Where(c => c.UserId, Compare.Equal, query.UserId)
			.QuerySingleAsync<EntryEntity>()
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: _ => F.True,
				none: _ => F.False
			);
	}
}
