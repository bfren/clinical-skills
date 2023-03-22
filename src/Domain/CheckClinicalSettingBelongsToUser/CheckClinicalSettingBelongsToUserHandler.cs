// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.Repositories;

namespace Domain.CheckClinicalSettingBelongsToUser;

/// <summary>
/// Check a clinical setting belongs to a user
/// </summary>
internal sealed class CheckClinicalSettingBelongsToUserHandler : QueryHandler<CheckClinicalSettingBelongsToUserQuery, bool>
{
	private IClinicalSettingRepository ClinicalSetting { get; init; }

	private ILog<CheckClinicalSettingBelongsToUserHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public CheckClinicalSettingBelongsToUserHandler(IClinicalSettingRepository clinicalSetting, ILog<CheckClinicalSettingBelongsToUserHandler> log) =>
		(ClinicalSetting, Log) = (clinicalSetting, log);

	/// <summary>
	/// Returns true if the clinical setting belongs to the user defined by <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<bool>> HandleAsync(CheckClinicalSettingBelongsToUserQuery query)
	{
		Log.Vrb("Checking clinical setting {ClinicalSettingId} belongs to user {UserId}.", query.ClinicalSettingId.Value, query.UserId.Value);
		return ClinicalSetting
			.StartFluentQuery()
			.Where(c => c.Id, Compare.Equal, query.ClinicalSettingId)
			.Where(c => c.UserId, Compare.Equal, query.UserId)
			.QuerySingleAsync<ClinicalSettingEntity>()
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: _ => F.True,
				none: _ => F.False
			);
	}
}
