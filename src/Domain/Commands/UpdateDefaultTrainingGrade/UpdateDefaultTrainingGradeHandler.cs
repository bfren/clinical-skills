// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Persistence.Repositories;

namespace Domain.Commands.UpdateDefaultTrainingGrade;

/// <summary>
/// Update default training grade
/// </summary>
internal sealed class UpdateDefaultTrainingGradeHandler : CommandHandler<UpdateDefaultTrainingGradeCommand>
{
	private IDispatcher Dispatcher { get; init; }

	private ILog<UpdateDefaultTrainingGradeHandler> Log { get; init; }

	private IUserSettingsRepository UserSettings { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="dispatcher"></param>
	/// <param name="log"></param>
	/// <param name="userSettings"></param>
	public UpdateDefaultTrainingGradeHandler(IDispatcher dispatcher, ILog<UpdateDefaultTrainingGradeHandler> log, IUserSettingsRepository userSettings) =>
		(Dispatcher, Log, UserSettings) = (dispatcher, log, userSettings);

	/// <summary>
	/// Update default training grade for user specified in <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override async Task<Maybe<bool>> HandleAsync(UpdateDefaultTrainingGradeCommand command)
	{
		if (command.DefaultTrainingGradeId is not null)
		{
			var check = await Dispatcher.SendAsync(
				new Queries.CheckTrainingGradeBelongsToUserQuery(command.UserId, command.DefaultTrainingGradeId)
			);
			if (!check.IsSome(out var value) || !value)
			{
				return F.None<bool, Messages.SaveSettingsCheckFailedMsg>();
			}
		}

		Log.Vrb("Updating Default Training Grade for {User}.", command.UserId.Value);
		return await UserSettings.UpdateAsync(command);
	}
}
