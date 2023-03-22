// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using Jeebs.Cqrs;
using Jeebs.Cryptography;
using Jeebs.Data;
using Persistence.StrongIds;

namespace Domain.SaveEntry.Internals;

/// <inheritdoc cref="UpdateEntryHandler"/>
/// <param name="Id">Entry ID</param>
/// <param name="Version">Entity Version</param>
/// <param name="DateOccurred"></param>
/// <param name="ClinicalSettingId"></param>
/// <param name="TrainingGradeId"></param>
/// <param name="PatientAge"></param>
/// <param name="CaseSummary"></param>
/// <param name="LearningPoints"></param>
/// <param name="LastUpdated"></param>
internal sealed record class UpdateEntryCommand(
	EntryId Id,
	long Version,
	DateTime DateOccurred,
	ClinicalSettingId ClinicalSettingId,
	TrainingGradeId TrainingGradeId,
	int PatientAge,
	Locked<string> CaseSummary,
	Locked<string> LearningPoints,
	DateTime LastUpdated
) : Command, IWithVersion<EntryId>
{
	/// <summary>
	/// Create from a <see cref="SaveEntryQuery"/>
	/// </summary>
	/// <param name="entryId"></param>
	/// <param name="query"></param>
	public UpdateEntryCommand(EntryId entryId, SaveEntryQuery query) : this(
		Id: entryId,
		Version: query.Version,
		DateOccurred: query.DateOccurred,
		ClinicalSettingId: query.ClinicalSettingId,
		TrainingGradeId: query.TrainingGradeId,
		PatientAge: query.PatientAge,
		CaseSummary: query.CaseSummary,
		LearningPoints: query.LearningPoints,
		LastUpdated: DateTime.Now
	)
	{ }

	/// <summary>
	/// Create empty for testing and model binding
	/// </summary>
	public UpdateEntryCommand() : this(new(), 0L, DateTime.MinValue, new(), new(), 0, new(), new(), DateTime.MinValue) { }
}
