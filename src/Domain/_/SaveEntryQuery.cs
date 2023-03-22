// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Domain.SaveEntry;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Cryptography;

namespace ClinicalSkills.Domain;

/// <inheritdoc cref="SaveEntryHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Id">Entry ID</param>
/// <param name="Version">Entity Verion</param>
/// <param name="DateOccurred"></param>
/// <param name="ClinicalSettingId"></param>
/// <param name="TrainingGradeId"></param>
/// <param name="PatientAge"></param>
/// <param name="CaseSummary"></param>
/// <param name="LearningPoints"></param>
public sealed record class SaveEntryQuery(
	AuthUserId UserId,
	EntryId? Id,
	long Version,
	DateTime DateOccurred,
	ClinicalSettingId ClinicalSettingId,
	TrainingGradeId TrainingGradeId,
	int PatientAge,
	Locked<string> CaseSummary,
	Locked<string> LearningPoints
) : Query<EntryId>, IWithUserId
{
	/// <summary>
	/// Create with minimum required values (for new entries)
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="dateOccurred"></param>
	/// <param name="clinicalSettingId"></param>
	/// <param name="trainingGradeId"></param>
	/// <param name="patientAge"></param>
	/// <param name="caseSummary"></param>
	/// <param name="learningPoints"></param>
	public SaveEntryQuery(
		AuthUserId userId,
		DateTime dateOccurred,
		ClinicalSettingId clinicalSettingId,
		TrainingGradeId trainingGradeId,
		int patientAge,
		Locked<string> caseSummary,
		Locked<string> learningPoints
	) : this(
		UserId: userId,
		Id: null,
		Version: 0L,
		DateOccurred: dateOccurred,
		ClinicalSettingId: clinicalSettingId,
		TrainingGradeId: trainingGradeId,
		PatientAge: patientAge,
		CaseSummary: caseSummary,
		LearningPoints: learningPoints
	)
	{ }

	/// <summary>
	/// Create empty for model binding
	/// </summary>
	public SaveEntryQuery() : this(new(), DateTime.MinValue, new(), new(), 0, new(), new()) { }
}
