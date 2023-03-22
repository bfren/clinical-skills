// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain.SaveTrainingGrade.Internals;

/// <inheritdoc cref="CreateTrainingGradeHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Description">Description</param>
internal sealed record class CreateTrainingGradeQuery(
	AuthUserId UserId,
	string Description
) : Query<TrainingGradeId>, IWithUserId
{
	/// <summary>
	/// Create from <see cref="SaveTrainingGradeQuery"/>
	/// </summary>
	/// <param name="query"></param>
	public CreateTrainingGradeQuery(SaveTrainingGradeQuery query) : this(
		UserId: query.UserId,
		Description: query.Description
	)
	{ }
}