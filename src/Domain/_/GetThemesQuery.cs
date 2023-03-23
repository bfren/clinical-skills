// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using Domain.GetThemes;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain;

/// <inheritdoc cref="GetThemesHandler"/>
/// <param name="UserId"></param>
public sealed record class GetThemesQuery(
	AuthUserId UserId
) : Query<IEnumerable<ThemesModel>>;
