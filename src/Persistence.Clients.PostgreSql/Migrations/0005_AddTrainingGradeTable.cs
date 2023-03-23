// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using Persistence.Tables;
using SimpleMigrations;

namespace Persistence.Clients.PostgreSql.Migrations;

[Migration(5, "Add training grade table")]
public sealed class AddTrainingGradeTable : Migration
{
	private string Col(Func<TrainingGradeTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE IF NOT EXISTS {Constants.Schema}.{TrainingGradeTable.TableName}
		(
			{Col(x => x.Id)} integer NOT NULL GENERATED ALWAYS AS IDENTITY,
			{Col(x => x.Version)} integer NOT NULL DEFAULT 0,
			{Col(x => x.UserId)} integer NOT NULL,
			{Col(x => x.Name)} text COLLATE pg_catalog."en-GB-x-icu" NOT NULL,
			{Col(x => x.IsDisabled)} boolean NOT NULL DEFAULT false,
			CONSTRAINT {Col(x => x.Id)}_key PRIMARY KEY({Col(x => x.Id)})
		)
		TABLESPACE pg_default
		;
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS {Constants.Schema}.{TrainingGradeTable.TableName}
		;
		""");
}
