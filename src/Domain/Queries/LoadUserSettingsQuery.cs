// Mileage Tracker
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2022

using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain.Queries;

/// <inheritdoc cref="LoadUserSettings.LoadUserSettingsHandler"/>
/// <param name="Id"></param>
public sealed record class LoadUserSettingsQuery(
	AuthUserId Id
) : Query<UserSettings>;
