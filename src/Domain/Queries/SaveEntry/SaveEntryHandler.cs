// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveEntry;

/// <summary>
/// Save a entry - create if it doesn't exist, or update if it does
/// </summary>
internal sealed class SaveEntryHandler : QueryHandler<SaveEntryQuery, EntryId>
{
	private IDispatcher Dispatcher { get; init; }

	private IEntryRepository Entry { get; init; }

	private ILog<SaveEntryHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="dispatcher"></param>
	/// <param name="log"></param>
	public SaveEntryHandler(IEntryRepository clinicalSetting, IDispatcher dispatcher, ILog<SaveEntryHandler> log) =>
		(Entry, Dispatcher, Log) = (clinicalSetting, dispatcher, log);

	/// <summary>
	/// Save the entry belonging to user specified in <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override async Task<Maybe<EntryId>> HandleAsync(SaveEntryQuery query)
	{
		Log.Vrb("Saving Entry {Query}.", query);

		// Ensure the entry belongs to the user
		if (query.Id?.Value > 0)
		{
			var entryBelongsToUser = await Dispatcher
				.DispatchAsync(new CheckEntryBelongsToUserQuery(query.UserId, query.Id))
				.IsTrueAsync();

			if (!entryBelongsToUser)
			{
				return F.None<EntryId>(new Messages.EntryDoesNotBelongToUserMsg(query.UserId, query.Id));
			}
		}

		// Create or update entry
		return await Entry
			.StartFluentQuery()
			.Where(x => x.Id, Compare.Equal, query.Id)
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.QuerySingleAsync<EntryEntity>()
			.SwitchAsync(
				some: x => Dispatcher
					.DispatchAsync(new Internals.UpdateEntryCommand(x.Id, query))
					.BindAsync(_ => F.Some(x.Id)),
				none: () => Dispatcher
					.DispatchAsync(new Internals.CreateEntryQuery(query))
			);
	}
}
