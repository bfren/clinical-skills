// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Domain.TruncateEverything;
using Jeebs.Cqrs;

namespace ClinicalSkills.Domain;

/// <inheritdoc cref="TruncateEverythingHandler"/>
public sealed record class TruncateEverythingCommand : Command;
