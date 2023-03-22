// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Threading.Tasks;
using ClinicalSkills.Persistence.Repositories;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Cqrs;
using Jeebs.Logging;

namespace ClinicalSkills.Domain.SaveEntry.Internals;

/// <summary>
/// Create a new entry entity
/// </summary>
internal sealed class CreateEntryHandler : QueryHandler<CreateEntryQuery, EntryId>
{
	private IEntryRepository Entry { get; init; }

	private ILog<CreateEntryHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="entry"></param>
	/// <param name="log"></param>
	public CreateEntryHandler(IEntryRepository entry, ILog<CreateEntryHandler> log) =>
		(Entry, Log) = (entry, log);

	/// <summary>
	/// Create a new entry from <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<EntryId>> HandleAsync(CreateEntryQuery query)
	{
		Log.Vrb("Creating Entry: {Query}", query);
		var now = DateTime.Now;
		return Entry
			.CreateAsync(new()
			{
				UserId = query.UserId,
				DateOccurred = query.DateOccurred,
				ClinicalSettingId = query.ClinicalSettingId,
				TrainingGradeId = query.TrainingGradeId,
				PatientAge = query.PatientAge,
				CaseSummary = query.CaseSummary,
				LearningPoints = query.LearningPoints,
				Created = now,
				LastUpdated = now
			});
	}
}
