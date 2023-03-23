// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Data;
using Persistence.StrongIds;

namespace Persistence.Entities;

/// <summary>
/// User Settings entity
/// </summary>
public sealed record class UserSettingsEntity : IWithVersion<UserSettingsId>
{
	/// <summary>
	/// Settings ID
	/// </summary>
	public UserSettingsId Id { get; init; } = new();

	/// <summary>
	/// Version
	/// </summary>
	public long Version { get; set; }

	/// <summary>
	/// User ID
	/// </summary>
	public AuthUserId UserId { get; init; } = new();

	/// <summary>
	/// Default Clinical Setting to be used for new entries
	/// </summary>
	public ClinicalSettingId? DefaultClinicalSettingId { get; init; }

	/// <summary>
	/// Default Training Grade to be used for new entries
	/// </summary>
	public TrainingGradeId? DefaultTrainingGradeId { get; set; }
}
