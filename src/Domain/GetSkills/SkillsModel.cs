// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Persistence.StrongIds;
using StrongId;

namespace Domain.GetSkills;

/// <summary>
/// Skills model
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
public sealed record class SkillsModel(
	SkillId Id,
	string Name
) : IWithId<SkillId>;
