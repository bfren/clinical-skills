// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using Persistence.Tables;
using SimpleMigrations;

namespace Persistence.Clients.PostgreSql.Migrations;

[Migration(1, "Add entry table")]
public sealed class AddEntryTable : Migration
{
	private string Col(Func<EntryTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE IF NOT EXISTS {Constants.Schema}.{EntryTable.TableName}
		(
			{Col(x => x.Id)} integer NOT NULL GENERATED ALWAYS AS IDENTITY,
			{Col(x => x.Version)} integer NOT NULL DEFAULT 0,
			{Col(x => x.UserId)} integer NOT NULL,
			{Col(x => x.DateOccurred)} date NOT NULL,
			{Col(x => x.ClinicalSettingId)} integer NOT NULL,
			{Col(x => x.TrainingGradeId)} integer NOT NULL,
			{Col(x => x.PatientAge)} integer NOT NULL,
			{Col(x => x.CaseSummary)} text COLLATE pg_catalog."en-GB-x-icu" NOT NULL,
			{Col(x => x.LearningPoints)} text COLLATE pg_catalog."en-GB-x-icu" NOT NULL,
			{Col(x => x.Created)} timestamp with time zone,
			{Col(x => x.LastUpdated)} timestamp with time zone,
			CONSTRAINT {Col(x => x.Id)}_key PRIMARY KEY({Col(x => x.Id)})
		)
		TABLESPACE pg_default
		;
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS {Constants.Schema}.{EntryTable.TableName}
		;
		""");
}
