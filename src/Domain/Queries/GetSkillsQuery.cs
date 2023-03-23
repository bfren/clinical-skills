// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using Domain.Queries.GetSkills;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain.Queries;

/// <inheritdoc cref="GetSkillsHandler"/>
/// <param name="UserId"></param>
/// <param name="IncludeDisabled">If true, disabled skills will be included</param>
public sealed record class GetSkillsQuery(
	AuthUserId UserId,
	bool IncludeDisabled
) : Query<IEnumerable<SkillsModel>>;
