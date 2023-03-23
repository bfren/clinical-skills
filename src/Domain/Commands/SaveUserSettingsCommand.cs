// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Commands.SaveUserSettings;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain.Commands;

/// <inheritdoc cref="SaveUserSettingsHandler"/>
/// <param name="UserId"></param>
/// <param name="Settings"></param>
public sealed record class SaveUserSettingsCommand(
	AuthUserId UserId,
	UserSettings Settings
) : Command;
