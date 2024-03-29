// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Linq.Expressions;
using Domain;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Persistence;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;
using StrongId;

namespace Abstracts.CheckCanBeDeleted;

public abstract class CheckIsDefaultAsync_Tests
{
	public abstract Task Test00_Calls_FluentQuery_Where__With_Correct_Values();

	public abstract Task Test01_Calls_FluentQuery_ExecuteAsync__With_Correct_Values();

	public abstract Task Test02_Calls_FluentQuery_ExecuteAsync__Receives_Some__Returns_True_When_Ids_Match();

	public abstract Task Test03_Calls_FluentQuery_ExecuteAsync__Receives_Some__Returns_False_When_Ids_Do_Not_Match();

	public abstract Task Test04_Calls_FluentQuery_ExecuteAsync__Receives_None__Returns_None();

	internal abstract class TestHandlerBase<TQuery, THandler, TId> : TestHandlerBase<IUserSettingsRepository, UserSettingsEntity, UserSettingsId, THandler>
		where TQuery : Query<DeleteOperation>
		where THandler : QueryHandler<TQuery, DeleteOperation>
		where TId : LongId, new()
	{
		internal new abstract class SetupBase : SetupBase<Vars> { }

		internal new abstract class SetupBase<TVars> : TestHandlerBase<IUserSettingsRepository, UserSettingsEntity, UserSettingsId, THandler>.SetupBase<TVars>
			where TVars : Vars, new()
		{
			public delegate CheckIsDefault<TId> CheckIsDefaultAsyncMethod(THandler handler);

			internal async Task Test00(CheckIsDefaultAsyncMethod checkIsDefault)
			{
				// Arrange
				var (handler, v) = GetVars();
				var userId = LongId<AuthUserId>();
				var check = checkIsDefault(handler);

				// Act
				_ = await check(userId, LongId<TId>());

				// Assert
				v.Fluent.AssertCalls(
					c => FluentQueryHelper.AssertWhere<UserSettingsEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
					_ => { }
				);
			}

			internal async Task Test01(CheckIsDefaultAsyncMethod checkIsDefault, Expression<Func<UserSettingsEntity, TId?>> property)
			{
				// Arrange
				var (handler, v) = GetVars();
				var check = checkIsDefault(handler);

				// Act
				_ = await check(LongId<AuthUserId>(), LongId<TId>());

				// Assert
				v.Fluent.AssertCalls(
					_ => { },
					c => FluentQueryHelper.AssertExecute(c, property, false)
				);
			}

			internal async Task Test02(CheckIsDefaultAsyncMethod checkIsDefault)
			{
				// Arrange
				var (handler, v) = GetVars();
				var entityId = LongId<TId>();
				v.Fluent.ExecuteAsync<TId?>(aliasSelector: default!)
					.ReturnsForAnyArgs(entityId);
				var check = checkIsDefault(handler);

				// Act
				var result = await check(LongId<AuthUserId>(), entityId);

				// Assert
				var some = result.AssertSome();
				Assert.True(some);
			}

			internal async Task Test03(CheckIsDefaultAsyncMethod checkIsDefault)
			{
				// Arrange
				var (handler, v) = GetVars();
				var entityId = LongId<TId>();
				v.Fluent.ExecuteAsync<TId?>(aliasSelector: default!)
					.ReturnsForAnyArgs(entityId);
				var check = checkIsDefault(handler);

				// Act
				var result = await check(LongId<AuthUserId>(), LongId<TId>());

				// Assert
				var some = result.AssertSome();
				Assert.False(some);
			}

			internal async Task Test04(CheckIsDefaultAsyncMethod checkIsDefault)
			{
				// Arrange
				var (handler, v) = GetVars();
				var none = Create.None<TId?>();
				v.Fluent.ExecuteAsync<TId?>(aliasSelector: default!)
					.ReturnsForAnyArgs(none);
				var check = checkIsDefault(handler);

				// Act
				var result = await check(LongId<AuthUserId>(), LongId<TId>());

				// Assert
				result.AssertNone();
			}
		}
	}
}
