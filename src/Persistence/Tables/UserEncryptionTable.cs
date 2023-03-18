// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data.Attributes;
using Jeebs.Data.Map;

namespace ClinicalSkills.Persistence.Tables;

/// <summary>
/// User Encryption table
/// </summary>
public sealed record class UserEncryptionTable() : Table(Constants.Schema, TableName)
{
	/// <summary>
	/// Table name - used as a prefix for each column
	/// </summary>
	public static readonly string TableName = "user_encryption";

	/// <inheritdoc cref="Entities.UserEncryptionEntity.Id"/>
	[Id]
	public string Id =>
		$"{TableName}_id";

	/// <inheritdoc cref="Entities.UserEncryptionEntity.Version"/>
	[Version]
	public string Version =>
		$"{TableName}_version";

	/// <inheritdoc cref="Entities.UserEncryptionEntity.UserId"/>
	public string UserId =>
		$"{TableName}_user_id";

	/// <inheritdoc cref="Entities.UserEncryptionEntity.Key"/>
	public string Key =>
		$"{TableName}_key";
}
