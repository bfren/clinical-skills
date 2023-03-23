// Mileage Tracker
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2022

using Jeebs.Messages;

namespace Domain.Commands.UpdateDefaultClinicalSetting.Messages;

/// <summary>Pre-save Settings checks have failed</summary>
public sealed record class SaveSettingsCheckFailedMsg : Msg;
