// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Persistence.StrongIds;
using StrongId;

namespace Domain.Queries.GetClinicalSetting;

/// <summary>
/// ClinicalSetting model
/// </summary>
/// <param name="UserId"></param>
/// <param name="Id"></param>
/// <param name="Version"></param>
/// <param name="Name"></param>
/// <param name="IsDisabled"></param>
public sealed record class ClinicalSettingModel(
	AuthUserId UserId,
	ClinicalSettingId Id,
	long Version,
	string Name,
	bool IsDisabled
) : IWithUserId, IWithId<ClinicalSettingId>
{
	/// <summary>
	/// Create empty - for model binding
	/// </summary>
	public ClinicalSettingModel() : this(new(), new(), 0L, string.Empty, false) { }
}
