// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data.Attributes;
using Jeebs.Data.Map;

namespace Persistence.Tables;

/// <summary>
/// Theme table
/// </summary>
public sealed record class ThemeTable() : Table(Constants.Schema, TableName)
{
	/// <summary>
	/// Table name - used as a prefix for each column
	/// </summary>
	public static readonly string TableName = "theme";

	/// <inheritdoc cref="Entities.ThemeEntity.Id"/>
	[Id]
	public string Id =>
		$"{TableName}_id";

	/// <inheritdoc cref="Entities.ThemeEntity.Version"/>
	[Version]
	public string Version =>
		$"{TableName}_version";

	/// <inheritdoc cref="Entities.ThemeEntity.UserId"/>
	public string UserId =>
		$"{TableName}_user_id";

	/// <inheritdoc cref="Entities.ThemeEntity.Name"/>
	public string Name =>
		$"{TableName}_name";

	/// <inheritdoc cref="Entities.ThemeEntity.IsDisabled"/>
	public string IsDisabled =>
		$"{TableName}_is_disabled";
}
