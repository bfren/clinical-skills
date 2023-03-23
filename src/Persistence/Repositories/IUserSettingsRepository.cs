// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data;
using Persistence.Entities;
using Persistence.StrongIds;

namespace Persistence.Repositories;

/// <summary>
/// User Settings repository
/// </summary>
public interface IUserSettingsRepository : IRepository<UserSettingsEntity, UserSettingsId> { }
