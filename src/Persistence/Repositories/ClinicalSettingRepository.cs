// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.StrongIds;

namespace Persistence.Repositories;

/// <inheritdoc cref="IClinicalSettingRepository"/>
public sealed class ClinicalSettingRepository : Repository<ClinicalSettingEntity, ClinicalSettingId>, IClinicalSettingRepository
{
	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="db"></param>
	/// <param name="log"></param>
	public ClinicalSettingRepository(IDb db, ILog<ClinicalSettingRepository> log) : base(db, log) { }
}
