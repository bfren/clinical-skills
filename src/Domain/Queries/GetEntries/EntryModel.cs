// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using Jeebs.Cryptography;
using Jeebs.Data;
using Persistence.Entities;
using Persistence.StrongIds;

namespace Domain.Queries.GetEntries;

/// <inheritdoc cref="GetEntriesQuery"/>
public record class EntryModel : IWithVersion<EntryId>
{
	/// <inheritdoc cref="EntryEntity.Id"/>
	public EntryId Id { get; init; } = new();

	/// <inheritdoc cref="EntryEntity.Version"/>
	public long Version { get; init; }

	/// <inheritdoc cref="EntryEntity.DateOccurred"/>
	public DateTime DateOccurred { get; init; }

	/// <inheritdoc cref="EntryEntity.ClinicalSettingId"/>
	public ClinicalSettingId ClinicalSettingId { get; init; } = new();

	/// <inheritdoc cref="EntryEntity.TrainingGradeId"/>
	public TrainingGradeId TrainingGradeId { get; init; } = new();

	/// <inheritdoc cref="EntryEntity.PatientAge"/>
	public int PatientAge { get; init; }

	/// <inheritdoc cref="EntryEntity.CaseSummary"/>
	public Locked<string> CaseSummary { get; init; } = new();

	/// <inheritdoc cref="EntryEntity.LearningPoints"/>
	public Locked<string> LearningPoints { get; init; } = new();
}
