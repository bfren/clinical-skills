// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.StrongIds;

namespace Persistence.Repositories;

/// <inheritdoc cref="ITrainingGradeRepository"/>
public sealed class TrainingGradeRepository : Repository<TrainingGradeEntity, TrainingGradeId>, ITrainingGradeRepository
{
	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="db"></param>
	/// <param name="log"></param>
	public TrainingGradeRepository(IDb db, ILog<TrainingGradeRepository> log) : base(db, log) { }
}
