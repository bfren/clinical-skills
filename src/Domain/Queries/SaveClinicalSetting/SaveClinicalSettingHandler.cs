// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveClinicalSetting;

/// <summary>
/// Save a clinical setting - create if it doesn't exist, or update if it does
/// </summary>
internal sealed class SaveClinicalSettingHandler : QueryHandler<SaveClinicalSettingQuery, ClinicalSettingId>
{
	private IDispatcher Dispatcher { get; init; }

	private IClinicalSettingRepository ClinicalSetting { get; init; }

	private ILog<SaveClinicalSettingHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="dispatcher"></param>
	/// <param name="log"></param>
	public SaveClinicalSettingHandler(IClinicalSettingRepository clinicalSetting, IDispatcher dispatcher, ILog<SaveClinicalSettingHandler> log) =>
		(ClinicalSetting, Dispatcher, Log) = (clinicalSetting, dispatcher, log);

	/// <summary>
	/// Save the clinical setting belonging to user specified in <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override async Task<Maybe<ClinicalSettingId>> HandleAsync(SaveClinicalSettingQuery query)
	{
		Log.Vrb("Saving Clinical Setting {Query}.", query);

		// Ensure the clinical setting belongs to the user
		if (query.Id?.Value > 0)
		{
			var clinicalSettingBelongsToUser = await Dispatcher
				.SendAsync(new CheckClinicalSettingBelongsToUserQuery(query.UserId, query.Id))
				.IsTrueAsync();

			if (!clinicalSettingBelongsToUser)
			{
				return F.None<ClinicalSettingId>(new Messages.ClinicalSettingDoesNotBelongToUserMsg(query.UserId, query.Id));
			}
		}

		// Create or update clinical setting
		return await ClinicalSetting
			.StartFluentQuery()
			.Where(x => x.Id, Compare.Equal, query.Id)
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.QuerySingleAsync<ClinicalSettingEntity>()
			.SwitchAsync(
				some: x => Dispatcher
					.SendAsync(new Internals.UpdateClinicalSettingCommand(x.Id, query))
					.BindAsync(_ => F.Some(x.Id)),
				none: () => Dispatcher
					.SendAsync(new Internals.CreateClinicalSettingQuery(query))
			);
	}
}
