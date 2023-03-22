// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data.Attributes;
using Jeebs.Data.Map;

namespace Persistence.Tables;

/// <summary>
/// Entry Theme table
/// </summary>
public sealed record class EntryThemeTable() : Table(Constants.Schema, TableName)
{
	/// <summary>
	/// Table name - used as a prefix for each column
	/// </summary>
	public static readonly string TableName = "entry_theme";

	/// <inheritdoc cref="Entities.EntryThemeEntity.Id"/>
	[Id]
	public string Id =>
		$"{TableName}_id";

	/// <inheritdoc cref="Entities.EntryThemeEntity.EntryId"/>
	public string EntryId =>
		$"{TableName}_entry_id";

	/// <inheritdoc cref="Entities.EntryThemeEntity.ThemeId"/>
	public string ThemeId =>
		$"{TableName}_theme_id";
}
