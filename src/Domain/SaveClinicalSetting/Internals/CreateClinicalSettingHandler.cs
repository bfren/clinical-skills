// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using ClinicalSkills.Persistence.Repositories;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Cqrs;
using Jeebs.Logging;

namespace ClinicalSkills.Domain.SaveClinicalSetting.Internals;

/// <summary>
/// Create a new clinical setting entity
/// </summary>
internal sealed class CreateClinicalSettingHandler : QueryHandler<CreateClinicalSettingQuery, ClinicalSettingId>
{
	private IClinicalSettingRepository ClinicalSetting { get; init; }

	private ILog<CreateClinicalSettingHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public CreateClinicalSettingHandler(IClinicalSettingRepository clinicalSetting, ILog<CreateClinicalSettingHandler> log) =>
		(ClinicalSetting, Log) = (clinicalSetting, log);

	/// <summary>
	/// Create a new clinical setting from <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<ClinicalSettingId>> HandleAsync(CreateClinicalSettingQuery query)
	{
		Log.Vrb("Creating Clinical Setting: {Query}", query);
		return ClinicalSetting
			.CreateAsync(new()
			{
				UserId = query.UserId,
				Description = query.Description
			});
	}
}
