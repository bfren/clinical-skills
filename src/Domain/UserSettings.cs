// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Persistence.StrongIds;

namespace Domain;

/// <summary>
/// Represents a user's settings
/// </summary>
/// <param name="UserId">User ID</param>
/// <param name="Version">Version (for concurrency)</param>
/// <param name="DefaultClinicalSettingId">Default Clinical Setting ID</param>
/// <param name="DefaultTrainingGradeId">Default Training Grade ID</param>
public sealed record class UserSettings(
	long Version,
	ClinicalSettingId? DefaultClinicalSettingId,
	TrainingGradeId? DefaultTrainingGradeId
)
{
	/// <summary>
	/// User Settings ID
	/// </summary>
	public UserSettingsId? Id { get; init; }

	/// <summary>
	/// Create default settings object
	/// </summary>
	public UserSettings() : this(0L, null, null) { }
}
