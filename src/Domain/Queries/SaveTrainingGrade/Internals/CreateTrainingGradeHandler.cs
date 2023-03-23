// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTrainingGrade.Internals;

/// <summary>
/// Create a new training grade entity
/// </summary>
internal sealed class CreateTrainingGradeHandler : QueryHandler<CreateTrainingGradeQuery, TrainingGradeId>
{
	private ITrainingGradeRepository TrainingGrade { get; init; }

	private ILog<CreateTrainingGradeHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public CreateTrainingGradeHandler(ITrainingGradeRepository clinicalSetting, ILog<CreateTrainingGradeHandler> log) =>
		(TrainingGrade, Log) = (clinicalSetting, log);

	/// <summary>
	/// Create a new training grade from <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<TrainingGradeId>> HandleAsync(CreateTrainingGradeQuery query)
	{
		Log.Vrb("Creating Training Grade: {Query}", query);
		return TrainingGrade
			.CreateAsync(new()
			{
				UserId = query.UserId,
				Name = query.Name
			});
	}
}
