// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Npgsql;

namespace ClinicalSkills.Persistence.Clients.PostgreSql;

/// <summary>
/// Sqlite database client migrator
/// </summary>
public sealed class PostgreSqlMigrator : Migrator
{
	/// <inheritdoc/>
	public override bool MigrateTo(string connectionString, long? version)
	{
		using NpgsqlConnection dbConnection = new(connectionString);
		return MigrateTo(ClientType.PostgreSql, dbConnection, typeof(PostgreSqlMigrator).Assembly, version);
	}
}
