// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using ClinicalSkills.Persistence.Types;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.Sqlite.Migrations;

[Migration(7, "Add entry skill table")]
public sealed class AddEntrySkillTable : Migration
{
	private string Col(Func<EntrySkillTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE "{Constants.Schema}.{EntrySkillTable.TableName}" (
			"{Col(e => e.Id)}" INTEGER NOT NULL UNIQUE,
			"{Col(e => e.EntryId)}" INTEGER NOT NULL,
			"{Col(e => e.SkillId)}" INTEGER NOT NULL,
			PRIMARY KEY("{Col(e => e.Id)}" AUTOINCREMENT)
		);
		CREATE UNIQUE INDEX "entry_skills" ON "{Constants.Schema}.{EntrySkillTable.TableName}" (
			"{Col(e => e.EntryId)}" ASC,
			"{Col(e => e.SkillId)}" ASC
		);
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS "{Constants.Schema}.{EntrySkillTable.TableName}";
		DROP INDEX IF EXISTS "entry_skills";
		""");
}
