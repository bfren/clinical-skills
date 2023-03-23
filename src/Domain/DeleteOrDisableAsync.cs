// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Auth.Data;
using Persistence;
using StrongId;

namespace Domain;

/// <summary>
/// Delegate to abstract deleting or disabling an item
/// </summary>
/// <typeparam name="TId">Item ID</typeparam>
/// <param name="userId">User ID</param>
/// <param name="entityId">Entity ID</param>
/// <param name="operation">Delete Operation to perform</param>
public delegate Task<Maybe<bool>> DeleteOrDisable<TId>(AuthUserId userId, TId entityId, DeleteOperation operation)
	where TId : LongId;

/// <summary>
/// Delegate to abstract determining whether or not the item is selected as default in settings
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <param name="userId"></param>
/// <param name="entityId"></param>
public delegate Task<Maybe<bool>> CheckIsDefault<TId>(AuthUserId userId, TId entityId)
	where TId : LongId;

/// <summary>
/// Delegate to abstract counting the number of entries with an item
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <param name="userId"></param>
/// <param name="entityId"></param>
public delegate Task<Maybe<long>> CountEntriesWith<TId>(AuthUserId userId, TId entityId)
	where TId : LongId;
