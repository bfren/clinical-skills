// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Persistence.Repositories;

namespace Domain.Commands.SaveUserSettings.Internals;

/// <summary>
/// Create settings for user
/// </summary>
internal sealed class CreateUserSettingsHandler : CommandHandler<CreateUserSettingsCommand>
{
	private ILog<CreateUserSettingsHandler> Log { get; init; }

	private IUserSettingsRepository UserSettings { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="log"></param>
	/// <param name="userSettings"></param>
	public CreateUserSettingsHandler(ILog<CreateUserSettingsHandler> log, IUserSettingsRepository userSettings) =>
		(Log, UserSettings) = (log, userSettings);

	/// <summary>
	/// Create settings for user specified in <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override Task<Maybe<bool>> HandleAsync(CreateUserSettingsCommand command)
	{
		Log.Vrb("Creating new settings for user {UserId}.", command.UserId.Value);
		return UserSettings
			.CreateAsync(new()
			{
				UserId = command.UserId,
				DefaultClinicalSettingId = command.Settings.DefaultClinicalSettingId,
				DefaultTrainingGradeId = command.Settings.DefaultTrainingGradeId
			})
			.AuditAsync(some: x => Log.Vrb("Created settings {SettingsId} for user {UserId}.", x.Value, command.UserId.Value))
			.BindAsync(_ => F.True);
	}
}
