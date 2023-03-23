// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain.Commands.SaveUserSettings.Internals;

/// <inheritdoc cref="CreateUserSettingsHandler"/>
/// <param name="UserId"></param>
/// <param name="Settings"></param>
internal sealed record class CreateUserSettingsCommand(
	AuthUserId UserId,
	UserSettings Settings
) : Command;
