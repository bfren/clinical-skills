// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data.Attributes;
using Jeebs.Data.Map;

namespace Persistence.Tables;

/// <summary>
/// Entry table
/// </summary>
public sealed record class EntryTable() : Table(Constants.Schema, TableName)
{
	/// <summary>
	/// Table name - used as a prefix for each column
	/// </summary>
	public static readonly string TableName = "entry";

	/// <inheritdoc cref="Entities.EntryEntity.Id"/>
	[Id]
	public string Id =>
		$"{TableName}_id";

	/// <inheritdoc cref="Entities.EntryEntity.Version"/>
	[Version]
	public string Version =>
		$"{TableName}_version";

	/// <inheritdoc cref="Entities.EntryEntity.UserId"/>
	public string UserId =>
		$"{TableName}_user_id";

	/// <inheritdoc cref="Entities.EntryEntity.DateOccurred"/>
	public string DateOccurred =>
		$"{TableName}_date_occurred";

	/// <inheritdoc cref="Entities.EntryEntity.ClinicalSettingId"/>
	public string ClinicalSettingId =>
		$"{TableName}_clinical_setting_id";

	/// <inheritdoc cref="Entities.EntryEntity.TrainingGradeId"/>
	public string TrainingGradeId =>
		$"{TableName}_training_grade_id";

	/// <inheritdoc cref="Entities.EntryEntity.PatientAge"/>
	public string PatientAge =>
		$"{TableName}_patient_age";

	/// <inheritdoc cref="Entities.EntryEntity.CaseSummary"/>
	public string CaseSummary =>
		$"{TableName}_case_summary";

	/// <inheritdoc cref="Entities.EntryEntity.LearningPoints"/>
	public string LearningPoints =>
		$"{TableName}_learning_points";

	/// <inheritdoc cref="Entities.EntryEntity.Created"/>
	public string Created =>
		$"{TableName}_created";

	/// <inheritdoc cref="Entities.EntryEntity.LastUpdated"/>
	public string LastUpdated =>
		$"{TableName}_last_updated";
}
