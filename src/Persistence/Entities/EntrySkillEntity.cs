// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Persistence.StrongIds;
using StrongId;

namespace Persistence.Entities;

/// <summary>
/// Entry Skill entity (for many-to-many mapping)
/// </summary>
public sealed record class EntrySkillEntity : IWithId<EntrySkillId>
{
	/// <summary>
	/// Entry Skill ID
	/// </summary>
	public EntrySkillId Id { get; init; } = new();

	/// <summary>
	/// Entry ID
	/// </summary>
	public EntryId EntryId { get; init; } = new();

	/// <summary>
	/// Skill ID
	/// </summary>
	public SkillId SkillId { get; init; } = new();
}
