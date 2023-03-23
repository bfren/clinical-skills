// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.GetClinicalSetting;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain.Queries;

/// <inheritdoc cref="GetClinicalSettingHandler"/>
/// <param name="UserId"></param>
/// <param name="ClinicalSettingId"></param>
public sealed record class GetClinicalSettingQuery(
	AuthUserId UserId,
	ClinicalSettingId ClinicalSettingId
) : Query<ClinicalSettingModel>;
