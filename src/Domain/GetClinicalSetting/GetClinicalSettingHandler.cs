// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using MaybeF.Caching;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.GetClinicalSetting;

/// <summary>
/// Get a clinical setting
/// </summary>
internal sealed class GetClinicalSettingHandler : QueryHandler<GetClinicalSettingQuery, ClinicalSettingModel>
{
	private IMaybeCache<ClinicalSettingId> Cache { get; init; }

	private IClinicalSettingRepository ClinicalSetting { get; init; }

	private ILog<GetClinicalSettingHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="cache"></param>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public GetClinicalSettingHandler(IMaybeCache<ClinicalSettingId> cache, IClinicalSettingRepository clinicalSetting, ILog<GetClinicalSettingHandler> log) =>
		(Cache, ClinicalSetting, Log) = (cache, clinicalSetting, log);

	/// <summary>
	/// Get the specified clinical setting if it belongs to the user
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<ClinicalSettingModel>> HandleAsync(GetClinicalSettingQuery query)
	{
		if (query.ClinicalSettingId is null || query.ClinicalSettingId.Value == 0)
		{
			return F.None<ClinicalSettingModel, Messages.ClinicalSettingIdIsNullMsg>().AsTask();
		}

		return Cache
			.GetOrCreateAsync(
				key: query.ClinicalSettingId,
				valueFactory: () =>
				{
					Log.Vrb("Get Clinical Setting: {Query}.", query);
					return ClinicalSetting.StartFluentQuery()
						.Where(x => x.Id, Compare.Equal, query.ClinicalSettingId)
						.Where(x => x.UserId, Compare.Equal, query.UserId)
						.QuerySingleAsync<ClinicalSettingModel>();
				}
			)
			.SwitchIfAsync(
				check: x => x.UserId == query.UserId,
				ifFalse: _ => F.None<ClinicalSettingModel, Messages.ClinicalSettingDoesNotBelongToUserMsg>()
			);
	}
}
