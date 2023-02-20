// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Data.SQLite;

namespace ClinicalSkills.Persistence.Clients.Sqlite;

/// <summary>
/// Sqlite database client migrator
/// </summary>
public sealed class SqliteMigrator : Migrator
{
	/// <inheritdoc/>
	public override bool MigrateTo(string connectionString, long? version)
	{
		using var dbConnection = new SQLiteConnection(connectionString);
		return MigrateTo(ClientType.Sqlite, dbConnection, typeof(SqliteMigrator).Assembly, version);
	}
}
