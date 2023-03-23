// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Persistence.StrongIds;
using StrongId;

namespace Domain.GetThemes;

/// <summary>
/// Themes model
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
public sealed record class ThemesModel(
	ThemeId Id,
	string Name
) : IWithId<ThemeId>;
