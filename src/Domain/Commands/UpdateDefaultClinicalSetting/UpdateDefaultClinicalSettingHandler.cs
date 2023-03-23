// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Persistence.Repositories;

namespace Domain.Commands.UpdateDefaultClinicalSetting;

/// <summary>
/// Update default clinical setting
/// </summary>
internal sealed class UpdateDefaultClinicalSettingHandler : CommandHandler<UpdateDefaultClinicalSettingCommand>
{
	private IDispatcher Dispatcher { get; init; }

	private ILog<UpdateDefaultClinicalSettingHandler> Log { get; init; }

	private IUserSettingsRepository UserSettings { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="dispatcher"></param>
	/// <param name="log"></param>
	/// <param name="userSettings"></param>
	public UpdateDefaultClinicalSettingHandler(IDispatcher dispatcher, ILog<UpdateDefaultClinicalSettingHandler> log, IUserSettingsRepository userSettings) =>
		(Dispatcher, Log, UserSettings) = (dispatcher, log, userSettings);

	/// <summary>
	/// Update default clinical setting for user specified in <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override async Task<Maybe<bool>> HandleAsync(UpdateDefaultClinicalSettingCommand command)
	{
		if (command.DefaultClinicalSettingId?.Value == 0)
		{
			command = command with { DefaultClinicalSettingId = null };
		}

		if (command.DefaultClinicalSettingId is not null)
		{
			var check = await Dispatcher.SendAsync(
				new Queries.CheckClinicalSettingBelongsToUserQuery(command.UserId, command.DefaultClinicalSettingId)
			);

			if (check.IsNone(out var _) || (check.IsSome(out var value) && !value))
			{
				return F.None<bool, Messages.SaveSettingsCheckFailedMsg>();
			}
		}

		Log.Vrb("Updating Default Clinical Setting for {User}.", command.UserId.Value);
		return await UserSettings.UpdateAsync(command);
	}
}
