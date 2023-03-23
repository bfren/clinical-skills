// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Persistence;

/// <summary>
/// Possible delete operations
/// </summary>
public enum DeleteOperation
{
	/// <summary>
	/// Nothing was done
	/// </summary>
	None = 0,

	/// <summary>
	/// Object was (or can be) deleted
	/// </summary>
	Delete = 1 << 0,

	/// <summary>
	/// Object was (or can be) disabled
	/// </summary>
	Disable = 1 << 1
}
