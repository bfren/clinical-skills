// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.Repositories;

namespace Domain.Queries.CheckTrainingGradeBelongsToUser;

/// <summary>
/// Check a training grade belongs to a user
/// </summary>
internal sealed class CheckTrainingGradeBelongsToUserHandler : QueryHandler<CheckTrainingGradeBelongsToUserQuery, bool>
{
	private ITrainingGradeRepository TrainingGrade { get; init; }

	private ILog<CheckTrainingGradeBelongsToUserHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public CheckTrainingGradeBelongsToUserHandler(ITrainingGradeRepository trainingGrade, ILog<CheckTrainingGradeBelongsToUserHandler> log) =>
		(TrainingGrade, Log) = (trainingGrade, log);

	/// <summary>
	/// Returns true if the training grade belongs to the user defined by <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<bool>> HandleAsync(CheckTrainingGradeBelongsToUserQuery query)
	{
		Log.Vrb("Checking training grade {TrainingGradeId} belongs to user {UserId}.", query.TrainingGradeId.Value, query.UserId.Value);
		return TrainingGrade
			.StartFluentQuery()
			.Where(c => c.Id, Compare.Equal, query.TrainingGradeId)
			.Where(c => c.UserId, Compare.Equal, query.UserId)
			.QuerySingleAsync<TrainingGradeEntity>()
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: _ => F.True,
				none: _ => F.False
			);
	}
}
