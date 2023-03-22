// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Domain.MigrateToLatest;
using Jeebs.Cqrs;

namespace ClinicalSkills.Domain;

/// <inheritdoc cref="MigrateToLatestHandler"/>
public sealed record class MigrateToLatestCommand : Command;
