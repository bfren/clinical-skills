// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicalSkills.Persistence.Repositories;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;

namespace ClinicalSkills.Domain.GetEntries;

/// <summary>
/// Get entries
/// </summary>
internal sealed class GetEntriesHandler : QueryHandler<GetEntriesQuery, IEnumerable<EntryModel>>
{
	private IEntryRepository Entry { get; init; }

	private ILog<GetEntriesHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="entry"></param>
	/// <param name="log"></param>
	public GetEntriesHandler(IEntryRepository entry, ILog<GetEntriesHandler> log) =>
		(Entry, Log) = (entry, log);

	/// <summary>
	/// Get journeys between dates specified in <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<IEnumerable<EntryModel>>> HandleAsync(GetEntriesQuery query)
	{
		var start = DateTime.SpecifyKind(query.Start, DateTimeKind.Unspecified);
		var end = DateTime.SpecifyKind(query.End, DateTimeKind.Unspecified);

		Log.Vrb("Getting Entries for {User} between {Start} and {End}.", query.UserId.Value, start, end);
		return Entry
			.StartFluentQuery()
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.Where(x => x.DateOccurred, Compare.MoreThanOrEqual, start)
			.Where(x => x.DateOccurred, Compare.LessThanOrEqual, end)
			.Sort(x => x.DateOccurred, SortOrder.Descending)
			.QueryAsync<EntryModel>();
	}
}
