// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using Domain.Queries.GetClinicalSettings;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain.Queries;

/// <inheritdoc cref="GetClinicalSettingsHandler"/>
/// <param name="UserId"></param>
/// <param name="IncludeDisabled">If true, disabled clinical settings will be included</param>
public sealed record class GetClinicalSettingsQuery(
	AuthUserId UserId,
	bool IncludeDisabled
) : Query<IEnumerable<ClinicalSettingsModel>>;
