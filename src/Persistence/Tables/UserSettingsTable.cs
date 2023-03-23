// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data.Attributes;
using Jeebs.Data.Map;

namespace Persistence.Tables;

/// <summary>
/// Settings Table definition
/// </summary>
public sealed record class UserSettingsTable() : Table(Constants.Schema, TableName)
{
	/// <summary>
	/// Table name - used as a prefix for each column
	/// </summary>
	public static readonly string TableName = "user_settings";

	/// <summary>
	/// Settings ID
	/// </summary>
	[Id]
	public string Id =>
		$"{TableName}_id";

	/// <summary>
	/// Settings Version
	/// </summary>
	[Version]
	public string Version =>
		$"{TableName}_version";

	/// <summary>
	/// Settings User ID
	/// </summary>
	public string UserId =>
		$"{TableName}_user_id";

	/// <summary>
	/// Settings Default Clinical Setting ID
	/// </summary>
	public string DefaultClinicalSettingId =>
		$"{TableName}_default_clinical_setting_id";

	/// <summary>
	/// Settings Default Training Grade ID
	/// </summary>
	public string DefaultTrainingGradeId =>
		$"{TableName}_default_training_grade_id";
}
