// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using Persistence.Tables;
using SimpleMigrations;

namespace Persistence.Clients.PostgreSql.Migrations;

[Migration(9, "Add user settings table")]
public sealed class AddUserSettingsTable : Migration
{
	private string Col(Func<UserSettingsTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE IF NOT EXISTS {Constants.Schema}.{UserSettingsTable.TableName}
		(
			{Col(x => x.Id)} integer NOT NULL GENERATED ALWAYS AS IDENTITY,
			{Col(x => x.Version)} integer NOT NULL DEFAULT 0,
			{Col(x => x.UserId)} integer NOT NULL,
			{Col(x => x.DefaultClinicalSettingId)} integer,
			{Col(x => x.DefaultTrainingGradeId)} integer,
			CONSTRAINT {Col(x => x.Id)}_key PRIMARY KEY({Col(x => x.Id)})
		)
		TABLESPACE pg_default
		;
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS {Constants.Schema}.{UserSettingsTable.TableName}
		;
		""");
}
