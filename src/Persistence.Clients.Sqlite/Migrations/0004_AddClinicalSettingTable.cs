// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using ClinicalSkills.Persistence.Types;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.Sqlite.Migrations;

[Migration(4, "Add clinical setting table")]
public sealed class AddClinicalSettingTable : Migration
{
	private string Col(Func<ClinicalSettingTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE "{Constants.Schema}.{ClinicalSettingTable.TableName}" (
			"{Col(e => e.Id)}" INTEGER NOT NULL UNIQUE,
			"{Col(e => e.Version)}" INTEGER NOT NULL DEFAULT 0,
			"{Col(e => e.UserId)}" INTEGER NOT NULL,
			"{Col(e => e.Description)}" TEXT NOT NULL,
			PRIMARY KEY("{Col(e => e.Id)}" AUTOINCREMENT)
		);
		CREATE INDEX "clinical_settings_by_user_id" ON "{Constants.Schema}.{ClinicalSettingTable.TableName}" (
			"{Col(e => e.UserId)}" ASC
		);
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS "{Constants.Schema}.{ClinicalSettingTable.TableName}";
		DROP INDEX IF EXISTS "clinical_settings_by_user_id";
		""");
}
