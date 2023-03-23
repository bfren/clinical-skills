// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Messages;

namespace Domain.Queries.GetClinicalSetting.Messages;

/// <summary>Requested clinical setting does not belong to the user</summary>
public sealed record class ClinicalSettingDoesNotBelongToUserMsg : Msg;
