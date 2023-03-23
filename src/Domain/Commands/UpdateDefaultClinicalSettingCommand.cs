// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;
using StrongId;

namespace Domain.Commands;

/// <inheritdoc cref="UpdateDefaultClinicalSetting.UpdateDefaultClinicalSettingHandler"/>
/// <param name="UserId"></param>
/// <param name="Id"></param>
/// <param name="Version"></param>
/// <param name="DefaultClinicalSettingId"></param>
public sealed record class UpdateDefaultClinicalSettingCommand(
	AuthUserId UserId,
	UserSettingsId Id,
	long Version,
	ClinicalSettingId? DefaultClinicalSettingId
) : Command, IWithId<UserSettingsId>, IWithUserId;
