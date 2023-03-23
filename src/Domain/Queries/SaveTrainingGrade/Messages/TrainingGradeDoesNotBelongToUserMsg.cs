// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Messages;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTrainingGrade.Messages;

/// <summary>Training Grade does not belong to a user</summary>
/// <param name="UserId"></param>
/// <param name="TrainingGradeId"></param>
public sealed record class TrainingGradeDoesNotBelongToUserMsg(
	AuthUserId UserId,
	TrainingGradeId TrainingGradeId
) : Msg;
