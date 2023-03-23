// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Repositories;

namespace Domain.GetEntries;

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
		Log.Vrb("Getting Entries for {User} matching {Query}.", query.UserId.Value, query);

		// Start query
		var fluent = Entry.StartFluentQuery()
			.Where(x => x.UserId, Compare.Equal, query.UserId);

		// Add dates from / to
		if (query.DateOccurredFrom is DateTime dateOccurredFrom)
		{
			dateOccurredFrom = DateTime.SpecifyKind(dateOccurredFrom, DateTimeKind.Unspecified);
			fluent = fluent.Where(x => x.DateOccurred, Compare.MoreThanOrEqual, dateOccurredFrom);
		}

		if (query.DateOccurredTo is DateTime dateOccurredTo)
		{
			dateOccurredTo = DateTime.SpecifyKind(dateOccurredTo, DateTimeKind.Unspecified);
			fluent = fluent.Where(x => x.DateOccurred, Compare.LessThanOrEqual, dateOccurredTo);
		}

		// Patient age from / to
		if (query.PatientAgeFrom is int patientAgeFrom)
		{
			fluent = fluent.Where(x => x.PatientAge, Compare.MoreThanOrEqual, patientAgeFrom);
		}

		if (query.PatientAgeTo is int patientAgeTo)
		{
			fluent = fluent.Where(x => x.PatientAge, Compare.LessThanOrEqual, patientAgeTo);
		}

		// Add sort and execute query
		return fluent
			.Sort(x => x.DateOccurred, SortOrder.Descending)
			.QueryAsync<EntryModel>();
	}
}
