// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using ClinicalSkills.Domain.GetEntries;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace ClinicalSkills.Domain;

/// <inheritdoc cref="GetEntriesHandler"/>
/// <param name="UserId"></param>
/// <param name="Start"></param>
/// <param name="End"></param>
public sealed record class GetEntriesQuery(
	AuthUserId UserId,
	DateTime Start,
	DateTime End
) : Query<IEnumerable<EntryModel>>;
