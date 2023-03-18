// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Data;

namespace ClinicalSkills.Persistence.Entities;

/// <summary>
/// Skill entity
/// </summary>
public sealed record class SkillEntity : IWithVersion<SkillId>
{
	/// <summary>
	/// Skill ID
	/// </summary>
	public SkillId Id { get; init; } = new();

	/// <summary>
	/// Skill version
	/// </summary>
	public long Version { get; set; }

	/// <summary>
	/// User ID
	/// </summary>
	public AuthUserId UserId { get; init; } = new();

	/// <summary>
	/// Skill name
	/// </summary>
	public string Name { get; init; } = string.Empty;
}
