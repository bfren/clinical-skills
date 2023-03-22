// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using ClinicalSkills.Persistence.Repositories;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Cqrs;
using Jeebs.Logging;
using MaybeF.Caching;

namespace ClinicalSkills.Domain.SaveClinicalSetting.Internals;

/// <summary>
/// Update an existing clinical setting entity
/// </summary>
internal sealed class UpdateClinicalSettingHandler : CommandHandler<UpdateClinicalSettingCommand>
{
	private IMaybeCache<ClinicalSettingId> Cache { get; init; }

	private IClinicalSettingRepository Car { get; init; }

	private ILog<UpdateClinicalSettingHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="cache"></param>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public UpdateClinicalSettingHandler(IMaybeCache<ClinicalSettingId> cache, IClinicalSettingRepository clinicalSetting, ILog<UpdateClinicalSettingHandler> log) =>
		(Cache, Car, Log) = (cache, clinicalSetting, log);

	/// <summary>
	/// Update a clinical setting from <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override Task<Maybe<bool>> HandleAsync(UpdateClinicalSettingCommand command)
	{
		Log.Vrb("Updating Clinical Setting: {Command}", command);
		return Car
			.UpdateAsync(command)
			.IfSomeAsync(x => { if (x) { Cache.RemoveValue(command.Id); } });
	}
}
