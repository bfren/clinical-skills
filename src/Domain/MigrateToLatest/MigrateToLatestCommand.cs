// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;

namespace ClinicalSkills.Domain.MigrateToLatest;

/// <inheritdoc cref="MigrateToLatestHandler"/>
public sealed record class MigrateToLatestCommand : Command;
