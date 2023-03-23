// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.GetUserProfile;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain.Queries;

/// <inheritdoc cref="GetUserProfileHandler"/>
/// <param name="Id"></param>
public sealed record class GetUserProfileQuery(
	AuthUserId Id
) : Query<UserProfileModel>;
