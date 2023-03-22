// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using Persistence.Tables;
using SimpleMigrations;

namespace Persistence.Clients.PostgreSql.Migrations;

[Migration(7, "Add entry skill table")]
public sealed class AddEntrySkillTable : Migration
{
	private string Col(Func<EntrySkillTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE IF NOT EXISTS {Constants.Schema}.{EntrySkillTable.TableName}
		(
			{Col(e => e.Id)} integer NOT NULL GENERATED ALWAYS AS IDENTITY,
			{Col(e => e.EntryId)} integer NOT NULL,
			{Col(e => e.SkillId)} integer NOT NULL,
			CONSTRAINT {Col(c => c.Id)}_key PRIMARY KEY({Col(c => c.Id)})
		)
		TABLESPACE pg_default
		;
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS {Constants.Schema}.{EntrySkillTable.TableName}
		;
		""");
}
