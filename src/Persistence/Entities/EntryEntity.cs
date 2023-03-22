// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using Jeebs.Auth.Data;
using Jeebs.Cryptography;
using Jeebs.Data;
using Persistence.StrongIds;

namespace Persistence.Entities;

/// <summary>
/// Entry entity
/// </summary>
public sealed record class EntryEntity : IWithVersion<EntryId>
{
	/// <summary>
	/// Entry ID
	/// </summary>
	public EntryId Id { get; init; } = new();

	/// <summary>
	/// Entry version
	/// </summary>
	public long Version { get; set; }

	/// <summary>
	/// User ID
	/// </summary>
	public AuthUserId UserId { get; init; } = new();

	/// <summary>
	/// The date this entry occurred
	/// </summary>
	public DateTime DateOccurred { get; init; }

	/// <summary>
	/// Clinical Setting ID
	/// </summary>
	public ClinicalSettingId ClinicalSettingId { get; init; } = new();

	/// <summary>
	/// Training Grade ID
	/// </summary>
	public TrainingGradeId TrainingGradeId { get; init; } = new();

	/// <summary>
	/// The patient's age
	/// </summary>
	public int PatientAge { get; init; }

	/// <summary>
	/// Case summary - encrypted by user's encryption key
	/// </summary>
	public Locked<string> CaseSummary { get; init; } = new();

	/// <summary>
	/// Learning points - encrypted by user's encryption key
	/// </summary>
	public Locked<string> LearningPoints { get; init; } = new();

	/// <summary>
	/// Date / time the entry was created
	/// </summary>
	public DateTime Created { get; init; }

	/// <summary>
	/// Date / time the entry was last updated
	/// </summary>
	public DateTime LastUpdated { get; init; }
}
