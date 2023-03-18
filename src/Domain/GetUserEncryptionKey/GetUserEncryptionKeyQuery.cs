// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cqrs;

namespace ClinicalSkills.Domain.GetUserEncryptionKey;

/// <inheritdoc cref="GetUserEncryptionKeyHandler"/>
/// <param name="UserId"></param>
public sealed record class GetUserEncryptionKeyQuery(
	AuthUserId UserId,
	string Password
) : Query<string>;
