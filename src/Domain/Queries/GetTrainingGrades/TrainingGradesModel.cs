// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Persistence.StrongIds;
using StrongId;

namespace Domain.Queries.GetTrainingGrades;

/// <summary>
/// Clinical Settings model
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
public sealed record class TrainingGradesModel(
	TrainingGradeId Id,
	string Name
) : IWithId<TrainingGradeId>;
