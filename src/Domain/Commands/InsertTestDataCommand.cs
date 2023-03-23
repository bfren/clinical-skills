// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;

namespace Domain.Commands;

/// <inheritdoc cref="InsertTestData.InsertTestDataHandler"/>
public sealed record class InsertTestDataCommand : Command;
