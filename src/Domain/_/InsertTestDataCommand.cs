// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Domain.InsertTestData;
using Jeebs.Cqrs;

namespace ClinicalSkills.Domain;

/// <inheritdoc cref="InsertTestDataHandler"/>
public sealed record class InsertTestDataCommand : Command;
