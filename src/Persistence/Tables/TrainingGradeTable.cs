// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data.Attributes;
using Jeebs.Data.Map;

namespace Persistence.Tables;

/// <summary>
/// Training Grade table
/// </summary>
public sealed record class TrainingGradeTable() : Table(Constants.Schema, TableName)
{
	/// <summary>
	/// Table name - used as a prefix for each column
	/// </summary>
	public static readonly string TableName = "training_grade";

	/// <inheritdoc cref="Entities.TrainingGradeEntity.Id"/>
	[Id]
	public string Id =>
		$"{TableName}_id";

	/// <inheritdoc cref="Entities.TrainingGradeEntity.Version"/>
	[Version]
	public string Version =>
		$"{TableName}_version";

	/// <inheritdoc cref="Entities.TrainingGradeEntity.UserId"/>
	public string UserId =>
		$"{TableName}_user_id";

	/// <inheritdoc cref="Entities.TrainingGradeEntity.Name"/>
	public string Name =>
		$"{TableName}_name";

	/// <inheritdoc cref="Entities.TrainingGradeEntity.IsDisabled"/>
	public string IsDisabled =>
		$"{TableName}_is_disabled";
}
