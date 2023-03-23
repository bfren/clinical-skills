// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;
using StrongId;

namespace Domain.Commands;

/// <inheritdoc cref="UpdateDefaultTrainingGrade.UpdateDefaultTrainingGradeHandler"/>
/// <param name="UserId"></param>
/// <param name="Id"></param>
/// <param name="Version"></param>
/// <param name="DefaultTrainingGradeId"></param>
public sealed record class UpdateDefaultTrainingGradeCommand(
	AuthUserId UserId,
	UserSettingsId Id,
	long Version,
	TrainingGradeId? DefaultTrainingGradeId
) : Command, IWithId<UserSettingsId>, IWithUserId;
