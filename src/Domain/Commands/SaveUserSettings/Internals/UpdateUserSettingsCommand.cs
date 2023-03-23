// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;
using Persistence.Entities;

namespace Domain.Commands.SaveUserSettings.Internals;

/// <inheritdoc cref="UpdateUserSettingsHandler"/>
/// <param name="ExistingSettings"></param>
/// <param name="UpdatedSettings"></param>
internal sealed record class UpdateUserSettingsCommand(
	UserSettingsEntity ExistingSettings,
	UserSettings UpdatedSettings
) : Command;
