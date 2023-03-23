// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using Domain.GetSkills;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain;

/// <inheritdoc cref="GetSkillsHandler"/>
/// <param name="UserId"></param>
public sealed record class GetSkillsQuery(
	AuthUserId UserId
) : Query<IEnumerable<SkillsModel>>;
