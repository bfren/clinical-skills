// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain;

/// <summary>
/// Returns true if <paramref name="ThemeId"/> belongs to <paramref name="UserId"/>
/// </summary>
/// <param name="UserId">User ID</param>
/// <param name="ThemeId">Theme ID</param>
public sealed record class CheckThemeBelongsToUserQuery(
	AuthUserId UserId,
	ThemeId ThemeId
) : Query<bool>;
