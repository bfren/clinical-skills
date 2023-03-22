// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicalSkills.Persistence.Repositories;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;

namespace ClinicalSkills.Domain.GetRecentEntries;

/// <summary>
/// Get recent entries
/// </summary>
internal sealed class GetRecentEntriesHandler : QueryHandler<GetRecentEntriesQuery, IEnumerable<RecentEntryModel>>
{
	private IEntryRepository Entry { get; init; }

	private ILog<GetRecentEntriesHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="entry"></param>
	/// <param name="log"></param>
	public GetRecentEntriesHandler(IEntryRepository entry, ILog<GetRecentEntriesHandler> log) =>
		(Entry, Log) = (entry, log);

	/// <summary>
	/// Get recent entries for user in <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<IEnumerable<RecentEntryModel>>> HandleAsync(GetRecentEntriesQuery query)
	{
		Log.Vrb("Getting recent entries for user {UserId}.", query.UserId.Value);
		return Entry
			.StartFluentQuery()
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.Sort(x => x.DateOccurred, SortOrder.Descending)
			.Sort(x => x.Created, SortOrder.Descending)
			.Maximum(5)
			.QueryAsync<RecentEntryModel>();
	}
}
