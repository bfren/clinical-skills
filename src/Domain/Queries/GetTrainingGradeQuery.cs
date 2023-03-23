// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.GetTrainingGrade;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain.Queries;

/// <inheritdoc cref="GetTrainingGradeHandler"/>
/// <param name="UserId"></param>
/// <param name="TrainingGradeId"></param>
public sealed record class GetTrainingGradeQuery(
	AuthUserId UserId,
	TrainingGradeId TrainingGradeId
) : Query<TrainingGradeModel>;
