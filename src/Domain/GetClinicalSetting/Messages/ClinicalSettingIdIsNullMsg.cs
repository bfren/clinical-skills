// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Messages;

namespace Domain.GetClinicalSetting.Messages;

/// <summary>Requested ClinicalSettingId is not set</summary>
public sealed record class ClinicalSettingIdIsNullMsg : Msg;
