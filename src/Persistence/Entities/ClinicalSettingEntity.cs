// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Data;
using Persistence.StrongIds;

namespace Persistence.Entities;

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
	/// Clinical Setting name
	/// </summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>
	/// Whether or not this clinical setting has been disabled
	/// </summary>
	public bool IsDisabled { get; init; }
}
