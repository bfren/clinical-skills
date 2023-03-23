// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Data;
using Persistence.StrongIds;

namespace Persistence.Entities;

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
	/// Training Grade name
	/// </summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>
	/// Whether or not this training grade has been disabled
	/// </summary>
	public bool IsDisabled { get; init; }
}
