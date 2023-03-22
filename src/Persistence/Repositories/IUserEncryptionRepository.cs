// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Persistence.Entities;
using Persistence.StrongIds;

namespace Persistence.Repositories;

/// <summary>
/// User Encryption repository
/// </summary>
public interface IUserEncryptionRepository : IRepository<UserEncryptionEntity, UserEncryptionId> { }
