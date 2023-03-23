// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using Domain.Queries.GetRecentEntries;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain.Queries;

/// <inheritdoc cref="GetRecentEntriesHandler"/>
/// <param name="UserId"></param>
public sealed record class GetRecentEntriesQuery(
	AuthUserId UserId
) : Query<IEnumerable<RecentEntryModel>>;
