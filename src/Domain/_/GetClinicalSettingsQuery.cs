// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using Domain.GetClinicalSettings;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain;

/// <inheritdoc cref="GetClinicalSettingsHandler"/>
/// <param name="UserId"></param>
public sealed record class GetClinicalSettingsQuery(
	AuthUserId UserId
) : Query<IEnumerable<ClinicalSettingsModel>>;
