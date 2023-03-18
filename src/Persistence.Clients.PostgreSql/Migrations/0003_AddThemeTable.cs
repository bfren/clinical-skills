// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.PostgreSql.Migrations;

[Migration(3, "Add theme table")]
public sealed class AddThemeTable : Migration
{
	private string Col(Func<ThemeTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE IF NOT EXISTS {Constants.Schema}.{ThemeTable.TableName}
		(
			{Col(e => e.Id)} integer NOT NULL GENERATED ALWAYS AS IDENTITY,
			{Col(e => e.Version)} integer NOT NULL DEFAULT 0,
			{Col(e => e.UserId)} integer NOT NULL,
			{Col(e => e.Name)} text COLLATE pg_catalog."en-GB-x-icu" NOT NULL,
			CONSTRAINT {Col(c => c.Id)}_key PRIMARY KEY({Col(c => c.Id)})
		)
		TABLESPACE pg_default
		;
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS {Constants.Schema}.{ThemeTable.TableName}
		;
		""");
}