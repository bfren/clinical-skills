// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Jeebs.Logging;
using Persistence.Entities;
using Persistence.StrongIds;

namespace Persistence.Repositories;

/// <inheritdoc cref="IUserEncryptionRepository"/>
public sealed class UserEncryptionRepository : Repository<UserEncryptionEntity, UserEncryptionId>, IUserEncryptionRepository
{
	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="db"></param>
	/// <param name="log"></param>
	public UserEncryptionRepository(IDb db, ILog<UserEncryptionRepository> log) : base(db, log) { }
}
