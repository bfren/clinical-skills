// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain.Queries;

/// <inheritdoc cref="SaveTrainingGradeHandler"/>
/// <param name="UserId">User ID</param>
/// <param name="Id">Training Grade ID</param>
/// <param name="Version">Entity Verion</param>
/// <param name="Name">Name</param>
public sealed record class SaveTrainingGradeQuery(
	AuthUserId UserId,
	TrainingGradeId? Id,
	long Version,
	string Name
) : Query<TrainingGradeId>, IWithUserId
{
	/// <summary>
	/// Create with minimum required values (for new training grades)
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="name"></param>
	public SaveTrainingGradeQuery(AuthUserId userId, string name) : this(
		UserId: userId,
		Id: null,
		Version: 0L,
		Name: name
	)
	{ }

	/// <summary>
	/// Create empty for model binding
	/// </summary>
	public SaveTrainingGradeQuery() : this(new(), string.Empty) { }
}
