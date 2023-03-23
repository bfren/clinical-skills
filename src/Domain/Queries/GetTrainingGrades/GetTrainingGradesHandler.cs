// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using Persistence.Repositories;

namespace Domain.Queries.GetTrainingGrades;

/// <summary>
/// Get training grades
/// </summary>
internal sealed class GetTrainingGradesHandler : QueryHandler<GetTrainingGradesQuery, IEnumerable<TrainingGradesModel>>
{
	private ITrainingGradeRepository TrainingGrade { get; init; }

	private ILog<GetTrainingGradesHandler> Log { get; init; }

	/// <summary>
	/// Inject dependency
	/// </summary>
	/// <param name="clinicalSetting"></param>
	/// <param name="log"></param>
	public GetTrainingGradesHandler(ITrainingGradeRepository clinicalSetting, ILog<GetTrainingGradesHandler> log) =>
		(TrainingGrade, Log) = (clinicalSetting, log);

	/// <summary>
	/// Get training grades for the specified user, sorted by name
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<IEnumerable<TrainingGradesModel>>> HandleAsync(GetTrainingGradesQuery query)
	{
		if (query.UserId is null || query.UserId.Value == 0)
		{
			return F.None<IEnumerable<TrainingGradesModel>, Messages.UserIdIsNullMsg>().AsTask();
		}

		Log.Vrb("Get Training Grades for {User}.", query.UserId);
		return TrainingGrade
			.StartFluentQuery()
			.Where(x => x.UserId, Compare.Equal, query.UserId)
			.WhereIn(x => x.IsDisabled, query.IncludeDisabled ? new[] { true, false } : new[] { false })
			.Sort(x => x.Name, SortOrder.Ascending)
			.QueryAsync<TrainingGradesModel>();
	}
}
