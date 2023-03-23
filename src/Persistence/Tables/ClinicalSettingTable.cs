// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data.Attributes;
using Jeebs.Data.Map;

namespace Persistence.Tables;

/// <summary>
/// Clinical Setting table
/// </summary>
public sealed record class ClinicalSettingTable() : Table(Constants.Schema, TableName)
{
	/// <summary>
	/// Table name - used as a prefix for each column
	/// </summary>
	public static readonly string TableName = "clinical_setting";

	/// <inheritdoc cref="Entities.ClinicalSettingEntity.Id"/>
	[Id]
	public string Id =>
		$"{TableName}_id";

	/// <inheritdoc cref="Entities.ClinicalSettingEntity.Version"/>
	[Version]
	public string Version =>
		$"{TableName}_version";

	/// <inheritdoc cref="Entities.ClinicalSettingEntity.UserId"/>
	public string UserId =>
		$"{TableName}_user_id";

	/// <inheritdoc cref="Entities.ClinicalSettingEntity.Name"/>
	public string Name =>
		$"{TableName}_name";

	/// <inheritdoc cref="Entities.ClinicalSettingEntity.IsDisabled"/>
	public string IsDisabled =>
		$"{TableName}_is_disabled";
}
