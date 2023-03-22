// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Domain.SaveTrainingGrade;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace ClinicalSkills.Domain;

/// <inheritdoc cref="SaveTrainingGradeHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Id">Training Grade ID</param>
/// <param name="Version">Entity Verion</param>
/// <param name="Description">Description</param>
public sealed record class SaveTrainingGradeQuery(
	AuthUserId UserId,
	TrainingGradeId? Id,
	long Version,
	string Description
) : Query<TrainingGradeId>, IWithUserId
{
	/// <summary>
	/// Create with minimum required values (for new training grades)
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="description"></param>
	public SaveTrainingGradeQuery(AuthUserId userId, string description) : this(
		UserId: userId,
		Id: null,
		Version: 0L,
		Description: description
	)
	{ }

	/// <summary>
	/// Create empty for model binding
	/// </summary>
	public SaveTrainingGradeQuery() : this(new(), string.Empty) { }
}
