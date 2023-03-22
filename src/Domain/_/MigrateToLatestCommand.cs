// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.MigrateToLatest;
using Jeebs.Cqrs;

namespace Domain;

/// <inheritdoc cref="MigrateToLatestHandler"/>
public sealed record class MigrateToLatestCommand : Command;
