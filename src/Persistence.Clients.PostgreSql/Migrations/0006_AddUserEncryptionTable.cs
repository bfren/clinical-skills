// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.PostgreSql.Migrations;

[Migration(6, "Add user encryption table")]
public sealed class AddUserEncryptionTable : Migration
{
	private string Col(Func<UserEncryptionTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE IF NOT EXISTS {Constants.Schema}.{UserEncryptionTable.TableName}
		(
			{Col(e => e.Id)} integer NOT NULL GENERATED ALWAYS AS IDENTITY,
			{Col(e => e.Version)} integer NOT NULL DEFAULT 0,
			{Col(e => e.UserId)} integer NOT NULL,
			{Col(e => e.Key)} jsonb NOT NULL,
			CONSTRAINT {Col(e => e.Id)}_key PRIMARY KEY({Col(e => e.Id)}),
			CONSTRAINT {Col(e => e.UserId)}_unique UNIQUE({Col(e => e.UserId)})
		)
		TABLESPACE pg_default
		;
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS {Constants.Schema}.{UserEncryptionTable.TableName}
		;
		""");
}
