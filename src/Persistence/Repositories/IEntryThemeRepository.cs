// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.Types.StrongIds;
using Jeebs.Data;

namespace ClinicalSkills.Persistence.Repositories;

/// <summary>
/// Entry Theme repository
/// </summary>
public interface IEntryThemeRepository : IRepository<EntryThemeEntity, EntryThemeId> { }
