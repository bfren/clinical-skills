// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.Types.StrongIds;
using Jeebs.Data;

namespace ClinicalSkills.Persistence.Repositories;

/// <summary>
/// Skill repository
/// </summary>
public interface ISkillRepository : IRepository<SkillEntity, SkillId> { }
