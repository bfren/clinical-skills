// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;
using Jeebs.Data;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTheme.Internals;

/// <inheritdoc cref="UpdateThemeHandler"/>
/// <param name="Id">Theme ID</param>
/// <param name="Version">Entity Version</param>
/// <param name="Name">Name</param>
internal sealed record class UpdateThemeCommand(
	ThemeId Id,
	long Version,
	string Name
) : Command, IWithVersion<ThemeId>
{
	/// <summary>
	/// Create from a <see cref="SaveThemeQuery"/>
	/// </summary>
	/// <param name="trainingGradeId"></param>
	/// <param name="query"></param>
	public UpdateThemeCommand(ThemeId trainingGradeId, SaveThemeQuery query) : this(
		Id: trainingGradeId,
		Version: query.Version,
		Name: query.Name
	)
	{ }
}
