// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.CheckClinicalSettingCanBeDeleted;

/// <summary>
/// Check whether or not a clinical setting can be deleted
/// </summary>
internal sealed class CheckClinicalSettingCanBeDeletedHandler : QueryHandler<CheckClinicalSettingCanBeDeletedQuery, DeleteOperation>
{
	private IEntryRepository Entry { get; init; }

	private ILog<CheckClinicalSettingCanBeDeletedHandler> Log { get; init; }

	private IUserSettingsRepository UserSettings { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="entry"></param>
	/// <param name="log"></param>
	/// <param name="userSettings"></param>
	public CheckClinicalSettingCanBeDeletedHandler(IEntryRepository entry, ILog<CheckClinicalSettingCanBeDeletedHandler> log, IUserSettingsRepository userSettings) =>
		(Entry, Log, UserSettings) = (entry, log, userSettings);

	/// <summary>
	/// Check whether or not the clinical setting defined in <paramref name="query"/> can be deleted or disabled
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<DeleteOperation>> HandleAsync(CheckClinicalSettingCanBeDeletedQuery query) =>
		HandleAsync(query, CheckIsDefaultAsync, CountEntriesWithAsync);

	internal async Task<Maybe<DeleteOperation>> HandleAsync(
		CheckClinicalSettingCanBeDeletedQuery query,
		CheckIsDefault<ClinicalSettingId> checkIsDefault,
		CountEntriesWith<ClinicalSettingId> countJourneysWith
	)
	{
		Log.Vrb("Checking whether or not Clinical Setting {ClinicalSettingId} can be deleted.", query.Id.Value);

		// Check whether or not it is the default clinical setting for the user
		var defaultCarQuery = await checkIsDefault(query.UserId, query.Id);
		if (defaultCarQuery.IsSome(out var isDefaultCar) && isDefaultCar)
		{
			return F.None<DeleteOperation, Messages.ClinicalSettingIsDefaultClinicalSettingMsg>();
		}
		else if (defaultCarQuery.IsNone(out var reason))
		{
			return F.None<DeleteOperation>(reason);
		}

		// Check whether or not the clinical setting is used in one of the user's entries
		var journeysWithCarQuery = await countJourneysWith(query.UserId, query.Id);
		return journeysWithCarQuery.Bind(x => x switch
		{
			> 0 =>
				F.Some(DeleteOperation.Disable),

			0 =>
				F.Some(DeleteOperation.Delete),

			_ =>
				F.Some(DeleteOperation.None)
		});
	}

	/// <summary>
	/// Check whether or not <paramref name="carId"/> is the default clinical setting in a user's settings
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="carId"></param>
	internal Task<Maybe<bool>> CheckIsDefaultAsync(AuthUserId userId, ClinicalSettingId carId) =>
		UserSettings.StartFluentQuery()
			.Where(x => x.UserId, Compare.Equal, userId)
			.ExecuteAsync(x => x.DefaultClinicalSettingId)
			.BindAsync(x => F.Some(x == carId));

	/// <summary>
	/// Count the number of entries using <paramref name="carId"/>
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="carId"></param>
	internal Task<Maybe<long>> CountEntriesWithAsync(AuthUserId userId, ClinicalSettingId carId) =>
		Entry.StartFluentQuery()
			.Where(x => x.UserId, Compare.Equal, userId)
			.Where(x => x.ClinicalSettingId, Compare.Equal, carId)
			.CountAsync();
}
