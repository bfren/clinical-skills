// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain.Queries;

/// <inheritdoc cref="SaveTheme.SaveThemeHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Id">Theme ID</param>
/// <param name="Version">Entity Verion</param>
/// <param name="Name">Name</param>
public sealed record class SaveThemeQuery(
	AuthUserId UserId,
	ThemeId? Id,
	long Version,
	string Name
) : Query<ThemeId>, IWithUserId
{
	/// <summary>
	/// Create with minimum required values (for new themes)
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="name"></param>
	public SaveThemeQuery(AuthUserId userId, string name) : this(
		UserId: userId,
		Id: null,
		Version: 0L,
		Name: name
	)
	{ }

	/// <summary>
	/// Create empty for model binding
	/// </summary>
	public SaveThemeQuery() : this(new(), string.Empty) { }
}
