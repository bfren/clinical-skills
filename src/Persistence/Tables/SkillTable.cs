// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data.Attributes;
using Jeebs.Data.Map;

namespace Persistence.Tables;

/// <summary>
/// Skill table
/// </summary>
public sealed record class SkillTable() : Table(Constants.Schema, TableName)
{
	/// <summary>
	/// Table name - used as a prefix for each column
	/// </summary>
	public static readonly string TableName = "skill";

	/// <inheritdoc cref="Entities.SkillEntity.Id"/>
	[Id]
	public string Id =>
		$"{TableName}_id";

	/// <inheritdoc cref="Entities.SkillEntity.Version"/>
	[Version]
	public string Version =>
		$"{TableName}_version";

	/// <inheritdoc cref="Entities.SkillEntity.UserId"/>
	public string UserId =>
		$"{TableName}_user_id";

	/// <inheritdoc cref="Entities.SkillEntity.Name"/>
	public string Name =>
		$"{TableName}_name";

	/// <inheritdoc cref="Entities.SkillEntity.IsDisabled"/>
	public string IsDisabled =>
		$"{TableName}_is_disabled";
}
