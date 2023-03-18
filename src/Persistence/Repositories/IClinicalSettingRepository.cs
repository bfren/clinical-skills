// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Data;

namespace ClinicalSkills.Persistence.Repositories;

/// <summary>
/// Clinical Setting repository
/// </summary>
public interface IClinicalSettingRepository : IRepository<ClinicalSettingEntity, ClinicalSettingId> { }
