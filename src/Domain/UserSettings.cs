// Mileage Tracker
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2022

using Persistence.StrongIds;

namespace Domain;

/// <summary>
/// Represents a user's settings
/// </summary>
/// <param name="Id">User Settings ID</param>
/// <param name="Version">Version (for concurrency)</param>
/// <param name="DefaultClinicalSettingId">Default Clinical Setting ID</param>
/// <param name="DefaultTrainingGradeId">Default Training Grade ID</param>
public sealed record class UserSettings(
	UserSettingsId Id,
	long Version,
	ClinicalSettingId? DefaultClinicalSettingId,
	TrainingGradeId? DefaultTrainingGradeId
)
{
	/// <summary>
	/// Create default settings object
	/// </summary>
	public UserSettings() : this(new(), 0L, null, null) { }
}
