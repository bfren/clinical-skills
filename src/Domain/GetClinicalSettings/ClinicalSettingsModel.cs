// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Persistence.StrongIds;
using StrongId;

namespace Domain.GetClinicalSettings;

/// <summary>
/// Clinical Settings model
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
public sealed record class ClinicalSettingsModel(
	ClinicalSettingId Id,
	string Name
) : IWithId<ClinicalSettingId>;
