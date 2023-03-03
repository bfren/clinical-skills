// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.Sqlite.Migrations;

[Migration(5, "Add training grade table")]
public sealed class AddTrainingGradeTable : Migration
{
	private string Col(Func<TrainingGradeTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE "{TrainingGradeTable.TableName}" (
			"{Col(e => e.Id)}" INTEGER NOT NULL UNIQUE,
			"{Col(e => e.Version)}" INTEGER NOT NULL,
			"{Col(e => e.UserId)}" INTEGER NOT NULL,
			"{Col(e => e.Description)}" TEXT NOT NULL,
			PRIMARY KEY("{Col(e => e.Id)}" AUTOINCREMENT)
		);
		CREATE INDEX "training_grades_by_user_id" ON "{TrainingGradeTable.TableName}" (
			"{Col(e => e.UserId)}" ASC
		);
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS "{TrainingGradeTable.TableName}";
		DROP INDEX IF EXISTS "training_grades_by_user_id";
		""");
}
