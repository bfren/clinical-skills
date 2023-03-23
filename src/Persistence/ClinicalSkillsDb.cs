// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Config.Db;
using Jeebs.Data;
using Jeebs.Logging;
using Microsoft.Extensions.Options;
using Persistence.Entities;
using Persistence.Tables;

namespace Persistence;

/// <summary>
/// Clinical Skills database
/// </summary>
public sealed class ClinicalSkillsDb : Db
{
	/// <summary>
	/// Clinical Setting tabls
	/// </summary>
	public ClinicalSettingTable ClinicalSetting { get; init; } = new();

	/// <summary>
	/// Entry table
	/// </summary>
	public EntryTable Entry { get; init; } = new();

	/// <summary>
	/// Entry Skill table
	/// </summary>
	public EntrySkillTable EntrySkill { get; init; } = new();

	/// <summary>
	/// Entry Theme table
	/// </summary>
	public EntryThemeTable EntryTheme { get; init; } = new();

	/// <summary>
	/// Skill table
	/// </summary>
	public SkillTable Skill { get; init; } = new();

	/// <summary>
	/// Theme table
	/// </summary>
	public ThemeTable Theme { get; init; } = new();

	/// <summary>
	/// Training Grade table
	/// </summary>
	public TrainingGradeTable TrainingGrade { get; init; } = new();

	/// <summary>
	/// User Encryption table
	/// </summary>
	public UserEncryptionTable UserEncryption { get; init; } = new();

	/// <summary>
	/// User Settings table
	/// </summary>
	public UserSettingsTable UserSettings { get; init; } = new();

	/// <summary>
	/// Inject dependencies and map entities
	/// </summary>
	/// <param name="client"></param>
	/// <param name="config"></param>
	/// <param name="log"></param>
	public ClinicalSkillsDb(IDbClient client, IOptions<DbConfig> config, ILog<ClinicalSkillsDb> log) :
		base(client, config.Value.GetConnection(), log)
	{
		// Map entities
		_ = Map<ClinicalSettingEntity>.To(ClinicalSetting);
		_ = Map<EntryEntity>.To(Entry);
		_ = Map<EntrySkillEntity>.To(EntrySkill);
		_ = Map<EntryThemeEntity>.To(EntryTheme);
		_ = Map<SkillEntity>.To(Skill);
		_ = Map<ThemeEntity>.To(Theme);
		_ = Map<TrainingGradeEntity>.To(TrainingGrade);
		_ = Map<UserEncryptionEntity>.To(UserEncryption);
		_ = Map<UserSettingsEntity>.To(UserSettings);

		// Add type handlers
		client.Types.AddLockedTypeHandlers();
		client.Types.AddStrongIdTypeHandlers();
	}
}
