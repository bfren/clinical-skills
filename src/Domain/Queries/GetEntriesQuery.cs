// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using Domain.Queries.GetEntries;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain.Queries;

/// <inheritdoc cref="GetEntriesHandler"/>
/// <param name="UserId"></param>
/// <param name="DateOccurredFrom"></param>
/// <param name="DateOccurredTo"></param>
/// <param name="ClinicalSettingIds"></param>
/// <param name="TrainingGradeIds"></param>
/// <param name="PatientAgeFrom"></param>
/// <param name="PatientAgeTo"></param>
/// <param name="SkillIds"></param>
/// <param name="ThemeIds"></param>
public sealed record class GetEntriesQuery(
	AuthUserId UserId,
	DateTime? DateOccurredFrom,
	DateTime? DateOccurredTo,
	ClinicalSettingId[]? ClinicalSettingIds,
	TrainingGradeId[]? TrainingGradeIds,
	int? PatientAgeFrom,
	int? PatientAgeTo,
	SkillId[]? SkillIds,
	ThemeId[]? ThemeIds
) : Query<IEnumerable<EntryModel>>
{
	/// <summary>
	/// Create basic query with only User ID
	/// </summary>
	/// <param name="userId"></param>
	public GetEntriesQuery(AuthUserId userId) : this(userId, null, null, null, null, null, null, null, null) { }
}
