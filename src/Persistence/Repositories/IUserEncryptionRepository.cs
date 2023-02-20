// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.Types.StrongIds;
using Jeebs.Data;

namespace ClinicalSkills.Persistence.Repositories;

/// <summary>
/// User Encryption repository
/// </summary>
public interface IUserEncryptionRepository : IRepository<UserEncryptionEntity, UserEncryptionId> { }
