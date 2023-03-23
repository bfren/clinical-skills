// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Messages;

namespace Domain.Commands.DeleteClinicalSetting.Messages;

/// <summary>
/// Invalid delete operation
/// </summary>
public sealed record class ClinicalSettingCannotBeDeletedMsg : Msg;
