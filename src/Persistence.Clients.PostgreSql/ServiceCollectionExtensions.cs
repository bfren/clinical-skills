// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Clients.PostgreSql;

/// <summary>
/// <see cref="IServiceCollection"/> extensions
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Add migrator class to the DI container
	/// </summary>
	/// <param name="services"></param>
	public static IServiceCollection AddClinicalSkillsMigrator(this IServiceCollection services) =>
		services.AddTransient<Migrator, PostgreSqlMigrator>();
}
