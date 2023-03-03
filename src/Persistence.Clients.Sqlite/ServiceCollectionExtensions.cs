// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Auth.Data.Clients.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicalSkills.Persistence.Clients.Sqlite;

/// <summary>
/// <see cref="IServiceCollection"/> extensions
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Add data classes to the DI container
	/// </summary>
	/// <param name="services"></param>
	public static IServiceCollection AddData(this IServiceCollection services) =>
		ClinicalSkillsDb
			.AddServices(services)
			.AddAuthData<SqliteDbClient>(true)
			.AddTransient<Migrator, SqliteMigrator>();
}
