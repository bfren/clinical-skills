// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using Domain.Queries.GetTrainingGrades;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain.Queries;

/// <inheritdoc cref="GetTrainingGradesHandler"/>
/// <param name="UserId"></param>
/// <param name="IncludeDisabled">If true, disabled training grades will be included</param>
public sealed record class GetTrainingGradesQuery(
	AuthUserId UserId,
	bool IncludeDisabled
) : Query<IEnumerable<TrainingGradesModel>>;
