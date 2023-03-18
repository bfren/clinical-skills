// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.PostgreSql.Migrations;

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
			{Col(e => e.Id)} integer NOT NULL GENERATED ALWAYS AS IDENTITY,
			{Col(e => e.Version)} integer NOT NULL DEFAULT 0,
			{Col(e => e.UserId)} integer NOT NULL,
			{Col(e => e.DateOccurred)} date NOT NULL,
			{Col(e => e.ClinicalSettingId)} integer NOT NULL,
			{Col(e => e.TrainingGradeId)} integer NOT NULL,
			{Col(e => e.PatientAge)} integer NOT NULL,
			{Col(e => e.CaseSummary)} text COLLATE pg_catalog."en-GB-x-icu" NOT NULL,
			{Col(e => e.LearningPoints)} text COLLATE pg_catalog."en-GB-x-icu" NOT NULL,
			{Col(e => e.Created)} timestamp with time zone,
			{Col(e => e.LastUpdated)} timestamp with time zone,
			CONSTRAINT {Col(c => c.Id)}_key PRIMARY KEY({Col(c => c.Id)})
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
