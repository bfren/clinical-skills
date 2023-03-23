// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Messages;

namespace Domain.Queries.GetTrainingGrade.Messages;

/// <summary>Requested training gradae does not belong to the user</summary>
public sealed record class TrainingGradeDoesNotBelongToUserMsg : Msg;
