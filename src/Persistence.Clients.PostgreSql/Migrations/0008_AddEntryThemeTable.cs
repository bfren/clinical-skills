// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.PostgreSql.Migrations;

[Migration(8, "Add entry theme table")]
public sealed class AddEntryThemeTable : Migration
{
	private string Col(Func<EntryThemeTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE IF NOT EXISTS {Constants.Schema}.{EntryThemeTable.TableName}
		(
			{Col(e => e.Id)} integer NOT NULL GENERATED ALWAYS AS IDENTITY,
			{Col(e => e.EntryId)} integer NOT NULL,
			{Col(e => e.ThemeId)} integer NOT NULL,
			CONSTRAINT {Col(c => c.Id)}_key PRIMARY KEY({Col(c => c.Id)})
		)
		TABLESPACE pg_default
		;
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS {Constants.Schema}.{EntryThemeTable.TableName}
		;
		""");
}
