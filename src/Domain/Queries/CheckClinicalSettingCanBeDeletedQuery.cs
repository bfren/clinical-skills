// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence;
using Persistence.StrongIds;
using StrongId;

namespace Domain.Queries;

/// <inheritdoc cref="CheckClinicalSettingCanBeDeleted.CheckClinicalSettingCanBeDeletedHandler"/>
/// <param name="UserId"></param>
/// <param name="Id"></param>
public sealed record class CheckClinicalSettingCanBeDeletedQuery(
	AuthUserId UserId,
	ClinicalSettingId Id
) : Query<DeleteOperation>, IWithId<ClinicalSettingId>, IWithUserId;
