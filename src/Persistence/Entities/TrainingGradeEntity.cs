// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Data;

namespace ClinicalSkills.Persistence.Entities;

/// <summary>
/// Training Grade entity
/// </summary>
public sealed record class TrainingGradeEntity : IWithVersion<TrainingGradeId>
{
	/// <summary>
	/// Training Grade ID
	/// </summary>
	public TrainingGradeId Id { get; init; } = new();

	/// <summary>
	/// Training Grade version
	/// </summary>
	public long Version { get; set; }

	/// <summary>
	/// User ID
	/// </summary>
	public AuthUserId UserId { get; init; } = new();

	/// <summary>
	/// Training Grade description
	/// </summary>
	public string Description { get; init; } = string.Empty;
}
