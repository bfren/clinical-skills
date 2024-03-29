// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace Domain.Queries;

/// <inheritdoc cref="CreateUser.CreateUserHandler"/>
/// <param name="Name">User's name</param>
/// <param name="EmailAddress">Email address (for login)</param>
/// <param name="Password">Password (will be hashed)</param>
public sealed record class CreateUserQuery(
	string Name,
	string EmailAddress,
	string Password
) : Query<AuthUserId>;
