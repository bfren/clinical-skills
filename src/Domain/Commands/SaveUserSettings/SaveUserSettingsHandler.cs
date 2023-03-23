// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Commands.SaveUserSettings;

/// <summary>
/// Save settings for a user - create if they don't exist, or update if they do
/// </summary>
internal sealed class SaveUserSettingsHandler : CommandHandler<SaveUserSettingsCommand>
{
	private IDispatcher Dispatcher { get; init; }

	private ILog<SaveUserSettingsHandler> Log { get; init; }

	private IUserSettingsRepository UserSettings { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="dispatcher"></param>
	/// <param name="userSettings"></param>
	/// <param name="log"></param>
	public SaveUserSettingsHandler(IDispatcher dispatcher, ILog<SaveUserSettingsHandler> log, IUserSettingsRepository userSettings) =>
		(Dispatcher, Log, UserSettings) = (dispatcher, log, userSettings);

	/// <summary>
	/// Save the settings for user specified in <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override async Task<Maybe<bool>> HandleAsync(SaveUserSettingsCommand command)
	{
		// Ensure the car belongs to the user (or is null)
		var carBelongsToUser = await CheckClinicalSettingBelongsToUser(
			command.Settings.DefaultClinicalSettingId, command.UserId
		);

		// Ensure the place belongs to the user (or is null)
		var placeBelongsToUser = await CheckTrainingGradeBelongsToUser(
			command.Settings.DefaultTrainingGradeId, command.UserId
		);

		// If checks have failed, return with failure message
		if (!carBelongsToUser || !placeBelongsToUser)
		{
			return F.None<bool, Messages.SaveUserSettingsCheckFailedMsg>();
		}

		// Add or update user settings
		return await UserSettings
			.StartFluentQuery()
			.Where(s => s.UserId, Compare.Equal, command.UserId)
			.QuerySingleAsync<UserSettingsEntity>()
			.SwitchAsync(
				some: x => Dispatcher
					.SendAsync(new Internals.UpdateUserSettingsCommand(x, command.Settings)),
				none: () => Dispatcher
					.SendAsync(new Internals.CreateUserSettingsCommand(command.UserId, command.Settings))
			);
	}

	/// <summary>
	/// Returns true if <paramref name="carId"/> is null or belongs to <paramref name="userId"/>
	/// </summary>
	/// <param name="carId"></param>
	/// <param name="userId"></param>
	internal Task<bool> CheckClinicalSettingBelongsToUser(ClinicalSettingId? carId, AuthUserId userId) =>
		carId switch
		{
			ClinicalSettingId x =>
				Dispatcher
					.SendAsync(new Queries.CheckClinicalSettingBelongsToUserQuery(userId, x))
					.IsTrueAsync(),

			_ =>
				Task.FromResult(true)
		};

	/// <summary>
	/// Returns true if <paramref name="placeId"/> is null or belongs to <paramref name="userId"/>
	/// </summary>
	/// <param name="placeId"></param>
	/// <param name="userId"></param>
	internal Task<bool> CheckTrainingGradeBelongsToUser(TrainingGradeId? placeId, AuthUserId userId) =>
		placeId switch
		{
			TrainingGradeId x =>
				Dispatcher
					.SendAsync(new Queries.CheckTrainingGradeBelongsToUserQuery(userId, x))
					.IsTrueAsync(),

			_ =>
				Task.FromResult(true)
		};
}
