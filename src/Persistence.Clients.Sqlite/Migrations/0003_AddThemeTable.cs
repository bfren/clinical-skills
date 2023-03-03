// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using ClinicalSkills.Persistence.Types;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.Sqlite.Migrations;

[Migration(3, "Add theme table")]
public sealed class AddThemeTable : Migration
{
	private string Col(Func<ThemeTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE "{Constants.Schema}.{ThemeTable.TableName}" (
			"{Col(e => e.Id)}" INTEGER NOT NULL UNIQUE,
			"{Col(e => e.Version)}" INTEGER NOT NULL,
			"{Col(e => e.UserId)}" INTEGER NOT NULL,
			"{Col(e => e.Name)}" TEXT NOT NULL,
			PRIMARY KEY("{Col(e => e.Id)}" AUTOINCREMENT)
		);
		CREATE INDEX "themes_by_user_id" ON "{Constants.Schema}.{ThemeTable.TableName}" (
			"{Col(e => e.UserId)}" ASC
		);
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS "{Constants.Schema}.{ThemeTable.TableName}";
		DROP INDEX IF EXISTS "themes_by_user_id";
		""");
}
