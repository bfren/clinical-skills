// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.StrongIds;

namespace Persistence.Repositories;

/// <inheritdoc cref="IUserSettingsRepository"/>
public sealed class UserSettingsRepository : Repository<UserSettingsEntity, UserSettingsId>, IUserSettingsRepository
{
	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="db"></param>
	/// <param name="log"></param>
	public UserSettingsRepository(IDb db, ILog<UserSettingsRepository> log) : base(db, log) { }
}
