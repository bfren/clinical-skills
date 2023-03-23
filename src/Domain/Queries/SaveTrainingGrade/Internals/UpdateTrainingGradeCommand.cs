// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;
using Jeebs.Data;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTrainingGrade.Internals;

/// <inheritdoc cref="UpdateTrainingGradeHandler"/>
/// <param name="Id">Training Grade ID</param>
/// <param name="Version">Entity Version</param>
/// <param name="Name">Name</param>
internal sealed record class UpdateTrainingGradeCommand(
	TrainingGradeId Id,
	long Version,
	string Name
) : Command, IWithVersion<TrainingGradeId>
{
	/// <summary>
	/// Create from a <see cref="SaveTrainingGradeQuery"/>
	/// </summary>
	/// <param name="trainingGradeId"></param>
	/// <param name="query"></param>
	public UpdateTrainingGradeCommand(TrainingGradeId trainingGradeId, SaveTrainingGradeQuery query) : this(
		Id: trainingGradeId,
		Version: query.Version,
		Name: query.Name
	)
	{ }
}
