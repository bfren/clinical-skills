// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Types.StrongIds;
using StrongId;

namespace ClinicalSkills.Persistence.Entities;

/// <summary>
/// Entry Theme entity (for many-to-many mapping)
/// </summary>
public sealed record class EntryThemeEntity : IWithId<EntryThemeId>
{
	/// <summary>
	/// Entry Theme ID
	/// </summary>
	public EntryThemeId Id { get; init; } = new();

	/// <summary>
	/// Entry ID
	/// </summary>
	public EntryId EntryId { get; init; } = new();

	/// <summary>
	/// Theme ID
	/// </summary>
	public ThemeId ThemeId { get; init; } = new();
}
