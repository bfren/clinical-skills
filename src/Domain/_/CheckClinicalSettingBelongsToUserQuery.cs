// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.StrongIds;

namespace Domain;

/// <summary>
/// Returns true if <paramref name="ClinicalSettingId"/> belongs to <paramref name="UserId"/>
/// </summary>
/// <param name="UserId">User ID</param>
/// <param name="ClinicalSettingId">Clinical Setting ID</param>
public sealed record class CheckClinicalSettingBelongsToUserQuery(
	AuthUserId UserId,
	ClinicalSettingId ClinicalSettingId
) : Query<bool>;
