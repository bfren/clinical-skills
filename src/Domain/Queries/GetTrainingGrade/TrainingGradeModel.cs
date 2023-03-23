// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Persistence.StrongIds;
using StrongId;

namespace Domain.Queries.GetTrainingGrade;

/// <summary>
/// TrainingGrade model
/// </summary>
/// <param name="UserId"></param>
/// <param name="Id"></param>
/// <param name="Version"></param>
/// <param name="Name"></param>
/// <param name="IsDisabled"></param>
public sealed record class TrainingGradeModel(
	AuthUserId UserId,
	TrainingGradeId Id,
	long Version,
	string Name,
	bool IsDisabled
) : IWithUserId, IWithId<TrainingGradeId>
{
	/// <summary>
	/// Create empty - for model binding
	/// </summary>
	public TrainingGradeModel() : this(new(), new(), 0L, string.Empty, false) { }
}
