// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Messages;

namespace Domain.Commands.UpdateDefaultTrainingGrade.Messages;

/// <summary>Pre-save Settings checks have failed</summary>
public sealed record class SaveSettingsCheckFailedMsg : Msg;
