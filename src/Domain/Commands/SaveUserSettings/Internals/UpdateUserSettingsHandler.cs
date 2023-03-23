// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Persistence.Repositories;

namespace Domain.Commands.SaveUserSettings.Internals;

/// <summary>
/// Update settings for user
/// </summary>
internal sealed class UpdateUserSettingsHandler : CommandHandler<UpdateUserSettingsCommand>
{
	private ILog<UpdateUserSettingsHandler> Log { get; init; }

	private IUserSettingsRepository UserSettings { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="log"></param>
	/// <param name="userSettings"></param>
	public UpdateUserSettingsHandler(ILog<UpdateUserSettingsHandler> log, IUserSettingsRepository userSettings) =>
		(Log, UserSettings) = (log, userSettings);

	/// <summary>
	/// Update settings for user specified in <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override Task<Maybe<bool>> HandleAsync(UpdateUserSettingsCommand command)
	{
		Log.Vrb("Updating settings {SettingsId} for user {UserId}.", command.ExistingSettings.Id.Value, command.ExistingSettings.UserId.Value);
		return UserSettings
			.UpdateAsync(command.ExistingSettings with
			{
				Version = command.UpdatedSettings.Version,
				DefaultClinicalSettingId = command.UpdatedSettings.DefaultClinicalSettingId,
				DefaultTrainingGradeId = command.UpdatedSettings.DefaultTrainingGradeId
			});
	}
}
