// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using MaybeF.Caching;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.SaveTrainingGrade.Internals;

/// <summary>
/// Update an existing training grade entity
/// </summary>
internal sealed class UpdateTrainingGradeHandler : CommandHandler<UpdateTrainingGradeCommand>
{
	private IMaybeCache<TrainingGradeId> Cache { get; init; }

	private ITrainingGradeRepository TrainingGrade { get; init; }

	private ILog<UpdateTrainingGradeHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="cache"></param>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public UpdateTrainingGradeHandler(IMaybeCache<TrainingGradeId> cache, ITrainingGradeRepository clinicalSetting, ILog<UpdateTrainingGradeHandler> log) =>
		(Cache, TrainingGrade, Log) = (cache, clinicalSetting, log);

	/// <summary>
	/// Update a training grade from <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override Task<Maybe<bool>> HandleAsync(UpdateTrainingGradeCommand command)
	{
		Log.Vrb("Updating Training Grade: {Command}", command);
		return TrainingGrade
			.UpdateAsync(command)
			.IfSomeAsync(x => { if (x) { Cache.RemoveValue(command.Id); } });
	}
}
