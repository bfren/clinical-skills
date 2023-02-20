// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Types.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Data;

namespace ClinicalSkills.Persistence.Entities;

/// <summary>
/// Theme entity
/// </summary>
public sealed record class ThemeEntity : IWithVersion<ThemeId>
{
	/// <summary>
	/// Theme ID
	/// </summary>
	public ThemeId Id { get; init; } = new();

	/// <summary>
	/// Theme version
	/// </summary>
	public long Version { get; set; }

	/// <summary>
	/// User ID
	/// </summary>
	public AuthUserId UserId { get; init; } = new();

	/// <summary>
	/// Theme name
	/// </summary>
	public string Name { get; init; } = string.Empty;
}
