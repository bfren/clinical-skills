// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Types;
using Jeebs.Data.Attributes;
using Jeebs.Data.Map;

namespace ClinicalSkills.Persistence.Tables;

/// <summary>
/// Entry Skill table
/// </summary>
public sealed record class EntrySkillTable() : Table(Constants.Schema, TableName)
{
	/// <summary>
	/// Table name - used as a prefix for each column
	/// </summary>
	public static readonly string TableName = "entry_skill";

	/// <inheritdoc cref="Entities.EntrySkillEntity.Id"/>
	[Id]
	public string Id =>
		$"{TableName}_id";

	/// <inheritdoc cref="Entities.EntrySkillEntity.EntryId"/>
	public string EntryId =>
		$"{TableName}_entry_id";

	/// <inheritdoc cref="Entities.EntrySkillEntity.SkillId"/>
	public string SkillId =>
		$"{TableName}_skill_id";
}
