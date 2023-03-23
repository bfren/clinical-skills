// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Messages;

namespace Domain.Queries.CheckClinicalSettingCanBeDeleted.Messages;

/// <summary>
/// Requested clinical setting is the user's default clinical setting so cannot be deleted or disabled
/// </summary>
public sealed record class ClinicalSettingIsDefaultClinicalSettingMsg : Msg;
