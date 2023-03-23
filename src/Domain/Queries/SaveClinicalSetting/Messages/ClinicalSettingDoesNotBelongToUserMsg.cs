// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Messages;
using Persistence.StrongIds;

namespace Domain.Queries.SaveClinicalSetting.Messages;

/// <summary>Clinical Setting does not belong to a user</summary>
/// <param name="UserId"></param>
/// <param name="ClinicalSettingId"></param>
public sealed record class ClinicalSettingDoesNotBelongToUserMsg(
	AuthUserId UserId,
	ClinicalSettingId ClinicalSettingId
) : Msg;
