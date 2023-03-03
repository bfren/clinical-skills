// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using ClinicalSkills.Persistence.Tables;
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
		CREATE TABLE "{UserEncryptionTable.TableName}" (
			"{Col(e => e.Id)}" INTEGER NOT NULL UNIQUE,
			"{Col(e => e.Version)}" INTEGER NOT NULL,
			"{Col(e => e.UserId)}" INTEGER NOT NULL,
			"{Col(e => e.Key)}" TEXT NOT NULL,
			PRIMARY KEY("{Col(e => e.Id)}" AUTOINCREMENT)
		);
		CREATE INDEX "user_encryption_by_user_id" ON "{UserEncryptionTable.TableName}" (
			"{Col(e => e.Id)}" ASC
		);
		""");

	/// <summary>
	/// 1: Down
	/// </summary>
	protected override void Down() => Execute($"""
		DROP TABLE IF EXISTS "{UserEncryptionTable.TableName}";
		DROP INDEX IF EXISTS "user_encryption_by_user_id";
		""");
}
