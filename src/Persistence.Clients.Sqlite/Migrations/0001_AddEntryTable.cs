// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using ClinicalSkills.Persistence.Types;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.Sqlite.Migrations;

[Migration(1, "Add entry table")]
public sealed class AddEntryTable : Migration
{
	private string Col(Func<EntryTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE "{Constants.Schema}.{EntryTable.TableName}" (
			"{Col(e => e.Id)}" INTEGER NOT NULL UNIQUE,
			"{Col(e => e.Version)}" INTEGER NOT NULL,
			"{Col(e => e.UserId)}" INTEGER NOT NULL,
			"{Col(e => e.DateOccurred)}" TEXT NOT NULL,
			"{Col(e => e.ClinicalSettingId)}" INTEGER NOT NULL,
			"{Col(e => e.TrainingGradeId)}" INTEGER NOT NULL,
			"{Col(e => e.PatientAge)}" INTEGER NOT NULL,
			"{Col(e => e.CaseSummary)}" TEXT NOT NULL,
			"{Col(e => e.LearningPoints)}" TEXT NOT NULL,
			"{Col(e => e.Created)}" TEXT NOT NULL,
			"{Col(e => e.LastUpdated)}" TEXT NOT NULL,
			PRIMARY KEY("{Col(e => e.Id)}" AUTOINCREMENT)
		);
		CREATE INDEX "entries_by_user_id" ON "{Constants.Schema}.{EntryTable.TableName}" (
			"{Col(e => e.UserId)}" ASC
		);
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS "{Constants.Schema}.{EntryTable.TableName}";
		DROP INDEX IF EXISTS "entries_by_user_id";
		""");
}
