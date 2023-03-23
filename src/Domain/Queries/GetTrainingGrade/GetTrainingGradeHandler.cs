// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using MaybeF.Caching;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.GetTrainingGrade;

/// <summary>
/// Get a training grade
/// </summary>
internal sealed class GetTrainingGradeHandler : QueryHandler<GetTrainingGradeQuery, TrainingGradeModel>
{
	private IMaybeCache<TrainingGradeId> Cache { get; init; }

	private ITrainingGradeRepository TrainingGrade { get; init; }

	private ILog<GetTrainingGradeHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="cache"></param>
	/// <param name="log"></param>
	/// <param name="trainingGrade"></param>
	public GetTrainingGradeHandler(IMaybeCache<TrainingGradeId> cache, ITrainingGradeRepository trainingGrade, ILog<GetTrainingGradeHandler> log) =>
		(Cache, Log, TrainingGrade) = (cache, log, trainingGrade);

	/// <summary>
	/// Get the specified training grade if it belongs to the user
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<TrainingGradeModel>> HandleAsync(GetTrainingGradeQuery query)
	{
		if (query.TrainingGradeId is null || query.TrainingGradeId.Value == 0)
		{
			return F.None<TrainingGradeModel, Messages.TrainingGradeIdIsNullMsg>().AsTask();
		}

		return Cache
			.GetOrCreateAsync(
				key: query.TrainingGradeId,
				valueFactory: () =>
				{
					Log.Vrb("Get Training Grade: {Query}.", query);
					return TrainingGrade.StartFluentQuery()
						.Where(x => x.Id, Compare.Equal, query.TrainingGradeId)
						.Where(x => x.UserId, Compare.Equal, query.UserId)
						.QuerySingleAsync<TrainingGradeModel>();
				}
			)
			.SwitchIfAsync(
				check: x => x.UserId == query.UserId,
				ifFalse: _ => F.None<TrainingGradeModel, Messages.TrainingGradeDoesNotBelongToUserMsg>()
			);
	}
}
