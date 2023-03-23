// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTheme.Internals;

/// <inheritdoc cref="CreateThemeHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Name">Name</param>
internal sealed record class CreateThemeQuery(
	AuthUserId UserId,
	string Name
) : Query<ThemeId>, IWithUserId
{
	/// <summary>
	/// Create from <see cref="SaveThemeQuery"/>
	/// </summary>
	/// <param name="query"></param>
	public CreateThemeQuery(SaveThemeQuery query) : this(
		UserId: query.UserId,
		Name: query.Name
	)
	{ }
}
