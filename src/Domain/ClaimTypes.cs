// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace ClinicalSkills.Domain;

/// <summary>
/// User authentication Claim Type names
/// </summary>
public static class ClaimTypes
{
	/// <summary>
	/// User encryption key
	/// </summary>
	public static string EncryptionKey { get; } = "encryption-key";
}
