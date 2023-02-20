// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.Types.StrongIds;
using Jeebs.Data;
using Jeebs.Logging;

namespace ClinicalSkills.Persistence.Repositories;

/// <inheritdoc cref="ISkillRepository"/>
public sealed class SkillRepository : Repository<SkillEntity, SkillId>, ISkillRepository
{
	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="db"></param>
	/// <param name="log"></param>
	public SkillRepository(IDb db, ILog<SkillRepository> log) : base(db, log) { }
}
