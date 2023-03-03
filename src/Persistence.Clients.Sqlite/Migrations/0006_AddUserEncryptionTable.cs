// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
using ClinicalSkills.Persistence.Types;
using SimpleMigrations;

namespace ClinicalSkills.Persistence.Clients.Sqlite.Migrations;

[Migration(6, "Add user encryption table")]
public sealed class AddUserEncryptionTable : Migration
{
	private string Col(Func<UserEncryptionTable, string> selector) =>
		selector(new());

	/// <summary>
	/// 1: Up
	/// </summary>
	protected override void Up() => Execute($"""
		CREATE TABLE "{Constants.Schema}.{UserEncryptionTable.TableName}" (
			"{Col(e => e.Id)}" INTEGER NOT NULL UNIQUE,
			"{Col(e => e.Version)}" INTEGER NOT NULL DEFAULT 0,
			"{Col(e => e.Email)}" TEXT NOT NULL,
			"{Col(e => e.Key)}" TEXT NOT NULL,
			PRIMARY KEY("{Col(e => e.Id)}" AUTOINCREMENT)
		);
		CREATE UNIQUE INDEX "user_encryption_by_email" ON "{Constants.Schema}.{UserEncryptionTable.TableName}" (
			"{Col(e => e.Email)}" ASC
		);
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS "{Constants.Schema}.{UserEncryptionTable.TableName}";
		DROP INDEX IF EXISTS "user_encryption_by_user_id";
		""");
}
