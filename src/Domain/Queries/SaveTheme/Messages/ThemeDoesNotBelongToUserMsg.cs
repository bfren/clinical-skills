// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Messages;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTheme.Messages;

/// <summary>Theme does not belong to a user</summary>
/// <param name="UserId"></param>
/// <param name="ThemeId"></param>
public sealed record class ThemeDoesNotBelongToUserMsg(
	AuthUserId UserId,
	ThemeId ThemeId
) : Msg;
