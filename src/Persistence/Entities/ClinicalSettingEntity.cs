// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Data;

namespace ClinicalSkills.Persistence.Entities;

/// <summary>
/// Clinical Setting entity
/// </summary>
public sealed record class ClinicalSettingEntity : IWithVersion<ClinicalSettingId>
{
	/// <summary>
	/// Clinical Setting ID
	/// </summary>
	public ClinicalSettingId Id { get; init; } = new();

	/// <summary>
	/// Clinical Setting version
	/// </summary>
	public long Version { get; set; }

	/// <summary>
	/// User ID
	/// </summary>
	public AuthUserId UserId { get; init; } = new();

	/// <summary>
	/// Clinical Setting description
	/// </summary>
	public string Description { get; init; } = string.Empty;
}
