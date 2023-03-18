// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Cryptography;
using Jeebs.Data;

namespace ClinicalSkills.Persistence.Entities;

/// <summary>
/// User Encryption entity
/// </summary>
public sealed record class UserEncryptionEntity : IWithVersion<UserEncryptionId>
{
	/// <summary>
	/// User Encryption ID
	/// </summary>
	public UserEncryptionId Id { get; init; } = new();

	/// <summary>
	/// User Encryption version
	/// </summary>
	public long Version { get; set; }

	/// <summary>
	/// User ID
	/// </summary>
	public AuthUserId UserId { get; init; } = new();

	/// <summary>
	/// User Encryption key
	/// </summary>
	public Locked<string> Key { get; init; } = new();
}
