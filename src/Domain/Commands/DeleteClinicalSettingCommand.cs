// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain.Commands;

/// <inheritdoc cref="DeleteClinicalSetting.DeleteClinicalSettingHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Id">Clinical Setting ID</param>
public sealed record class DeleteClinicalSettingCommand(
	AuthUserId UserId,
	ClinicalSettingId Id
) : Command;
