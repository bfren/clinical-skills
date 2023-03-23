// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Repositories;

namespace Domain.Queries.GetClinicalSettings;

/// <summary>
/// Get clinical settings
/// </summary>
internal sealed class GetClinicalSettingsHandler : QueryHandler<GetClinicalSettingsQuery, IEnumerable<ClinicalSettingsModel>>
{
	private IClinicalSettingRepository ClinicalSetting { get; init; }

	private ILog<GetClinicalSettingsHandler> Log { get; init; }

	/// <summary>
	/// Inject dependency
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public GetClinicalSettingsHandler(IClinicalSettingRepository clinicalSetting, ILog<GetClinicalSettingsHandler> log) =>
		(ClinicalSetting, Log) = (clinicalSetting, log);

	/// <summary>
	/// Get clinical settings for the specified user, sorted by name
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<IEnumerable<ClinicalSettingsModel>>> HandleAsync(GetClinicalSettingsQuery query)
	{
		if (query.UserId is null || query.UserId.Value == 0)
		{
			return F.None<IEnumerable<ClinicalSettingsModel>, Messages.UserIdIsNullMsg>().AsTask();
		}

		Log.Vrb("Get Clinical Settings for {User}.", query.UserId);
		return ClinicalSetting
			.StartFluentQuery()
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.Sort(x => x.Name, SortOrder.Ascending)
			.QueryAsync<ClinicalSettingsModel>();
	}
}
