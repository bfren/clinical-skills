// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;

namespace ClinicalSkills.Domain;

/// <summary>
/// Specifies a class has a 'UserId' property
/// </summary>
public interface IWithUserId
{
	/// <summary>
	/// User ID
	/// </summary>
	AuthUserId UserId { get; init; }
}
