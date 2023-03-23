// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Messages;
using Persistence.StrongIds;
using StrongId;

namespace Domain.Commands.DeleteClinicalSetting.Messages;

/// <summary>
/// The clinical setting does not exist, or does not belong to the specified user
/// </summary>
/// <param name="UserId">User ID</param>
/// <param name="Id">Clinical Setting ID</param>
public sealed record class ClinicalSettingDoesNotExistMsg(AuthUserId UserId, ClinicalSettingId Id) : Msg, IWithUserId, IWithId<ClinicalSettingId>;
