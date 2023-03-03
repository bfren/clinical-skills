// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.Sqlite.Migrations;

[Migration(8, "Add entry theme table")]
public sealed class AddEntryThemeTable : Migration
{
	private string Col(Func<EntryThemeTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE "{EntryThemeTable.TableName}" (
			"{Col(e => e.Id)}" INTEGER NOT NULL UNIQUE,
			"{Col(e => e.EntryId)}" INTEGER NOT NULL,
			"{Col(e => e.ThemeId)}" INTEGER NOT NULL,
			PRIMARY KEY("{Col(e => e.Id)}" AUTOINCREMENT)
		);
		CREATE UNIQUE INDEX "entry_themes" ON "{EntryThemeTable.TableName}" (
			"{Col(e => e.EntryId)}" ASC,
			"{Col(e => e.ThemeId)}" ASC
		);
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS "{EntryThemeTable.TableName}";
		DROP INDEX IF EXISTS "entry_themes";
		""");
}
