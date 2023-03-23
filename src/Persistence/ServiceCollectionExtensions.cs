// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence;

/// <summary>
/// <see cref="IServiceCollection"/> extensions
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Add data classes to the DI container
	/// </summary>
	/// <param name="services"></param>
	public static IServiceCollection AddClinicalSkillsData(this IServiceCollection services)
	{
		// Add database
		_ = services.AddSingleton<IDb, ClinicalSkillsDb>();

		// Add repositories
		_ = services
			.AddTransient<IClinicalSettingRepository, ClinicalSettingRepository>()
			.AddTransient<IEntryRepository, EntryRepository>()
			.AddTransient<IEntrySkillRepository, EntrySkillRepository>()
			.AddTransient<IEntryThemeRepository, EntryThemeRepository>()
			.AddTransient<IUserSettingsRepository, UserSettingsRepository>()
			.AddTransient<ISkillRepository, SkillRepository>()
			.AddTransient<IThemeRepository, ThemeRepository>()
			.AddTransient<ITrainingGradeRepository, TrainingGradeRepository>()
			.AddTransient<IUserEncryptionRepository, UserEncryptionRepository>();

		// Return
		return services;
	}
}
