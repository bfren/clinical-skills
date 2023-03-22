// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Cryptography;
using Persistence.StrongIds;

namespace Domain.SaveEntry.Internals;

/// <inheritdoc cref="CreateEntryHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="DateOccurred"></param>
/// <param name="ClinicalSettingId"></param>
/// <param name="TrainingGradeId"></param>
/// <param name="PatientAge"></param>
/// <param name="CaseSummary"></param>
/// <param name="LearningPoints"></param>
internal sealed record class CreateEntryQuery(
	AuthUserId UserId,
	DateTime DateOccurred,
	ClinicalSettingId ClinicalSettingId,
	TrainingGradeId TrainingGradeId,
	int PatientAge,
	Locked<string> CaseSummary,
	Locked<string> LearningPoints
) : Query<EntryId>, IWithUserId
{
	/// <summary>
	/// Create from <see cref="SaveEntryQuery"/>
	/// </summary>
	/// <param name="query"></param>
	public CreateEntryQuery(SaveEntryQuery query) : this(
		UserId: query.UserId,
		DateOccurred: query.DateOccurred,
		ClinicalSettingId: query.ClinicalSettingId,
		TrainingGradeId: query.TrainingGradeId,
		PatientAge: query.PatientAge,
		CaseSummary: query.CaseSummary,
		LearningPoints: query.LearningPoints
	)
	{ }

	/// <summary>
	/// Create empty for testing and model binding
	/// </summary>
	public CreateEntryQuery() : this(new(), DateTime.MinValue, new(), new(), 0, new(), new()) { }
}
