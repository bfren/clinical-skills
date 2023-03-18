// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace ClinicalSkills.Domain.GetUserProfile;

/// <inheritdoc cref="GetUserProfileHandler"/>
/// <param name="Id"></param>
public sealed record class GetUserProfileQuery(
	AuthUserId Id
) : Query<UserProfileModel>;
