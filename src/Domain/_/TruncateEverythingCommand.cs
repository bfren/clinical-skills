// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.TruncateEverything;
using Jeebs.Cqrs;

namespace Domain;

/// <inheritdoc cref="TruncateEverythingHandler"/>
public sealed record class TruncateEverythingCommand : Command;
