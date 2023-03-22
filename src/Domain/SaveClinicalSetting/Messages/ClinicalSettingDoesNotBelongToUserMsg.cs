// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Messages;

namespace ClinicalSkills.Domain.SaveClinicalSetting.Messages;

/// <summary>Clinical Setting does not belong to a user</summary>
/// <param name="UserId"></param>
/// <param name="ClinicalSettingId"></param>
public sealed record class ClinicalSettingDoesNotBelongToUserMsg(
	AuthUserId UserId,
	ClinicalSettingId ClinicalSettingId
) : Msg;
