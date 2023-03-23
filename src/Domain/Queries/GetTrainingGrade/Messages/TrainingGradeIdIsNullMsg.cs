// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Messages;

namespace Domain.Queries.GetTrainingGrade.Messages;

/// <summary>Requested TrainingGradeId is not set</summary>
public sealed record class TrainingGradeIdIsNullMsg : Msg;
