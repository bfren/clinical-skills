// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTrainingGrade;

/// <summary>
/// Save a training grade - create if it doesn't exist, or update if it does
/// </summary>
internal sealed class SaveTrainingGradeHandler : QueryHandler<SaveTrainingGradeQuery, TrainingGradeId>
{
	private IDispatcher Dispatcher { get; init; }

	private ITrainingGradeRepository TrainingGrade { get; init; }

	private ILog<SaveTrainingGradeHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="dispatcher"></param>
	/// <param name="log"></param>
	public SaveTrainingGradeHandler(ITrainingGradeRepository clinicalSetting, IDispatcher dispatcher, ILog<SaveTrainingGradeHandler> log) =>
		(TrainingGrade, Dispatcher, Log) = (clinicalSetting, dispatcher, log);

	/// <summary>
	/// Save the training grade belonging to user specified in <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override async Task<Maybe<TrainingGradeId>> HandleAsync(SaveTrainingGradeQuery query)
	{
		Log.Vrb("Saving Training Grade {Query}.", query);

		// Ensure the training grade belongs to the user
		if (query.Id?.Value > 0)
		{
			var trainingGradeBelongsToUser = await Dispatcher
				.DispatchAsync(new CheckTrainingGradeBelongsToUserQuery(query.UserId, query.Id))
				.IsTrueAsync();

			if (!trainingGradeBelongsToUser)
			{
				return F.None<TrainingGradeId>(new Messages.TrainingGradeDoesNotBelongToUserMsg(query.UserId, query.Id));
			}
		}

		// Create or update training grade
		return await TrainingGrade
			.StartFluentQuery()
			.Where(x => x.Id, Compare.Equal, query.Id)
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.QuerySingleAsync<TrainingGradeEntity>()
			.SwitchAsync(
				some: x => Dispatcher
					.DispatchAsync(new Internals.UpdateTrainingGradeCommand(x.Id, query))
					.BindAsync(_ => F.Some(x.Id)),
				none: () => Dispatcher
					.DispatchAsync(new Internals.CreateTrainingGradeQuery(query))
			);
	}
}
