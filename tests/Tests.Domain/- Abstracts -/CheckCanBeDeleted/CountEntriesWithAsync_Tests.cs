// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using NSubstitute.Core;
using Persistence;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;
using StrongId;

namespace Abstracts.CheckCanBeDeleted;

public abstract class CountEntriesWithAsync_Tests
{
	public abstract Task Test00_Calls_FluentQuery_Where__With_Correct_Values();

	public abstract Task Test01_Calls_FluentQuery_CountAsync();

	public abstract Task Test02_Calls_FluentQuery_CountAsync__Returns_Result();

	internal abstract class TestHandlerBase<TQuery, THandler, TId> : TestHandlerBase<IEntryRepository, EntryEntity, EntryId, THandler>
		where TQuery : Query<DeleteOperation>
		where THandler : QueryHandler<TQuery, DeleteOperation>
		where TId : LongId, new()
	{
		internal new abstract class SetupBase : SetupBase<Vars> { }

		internal new abstract class SetupBase<TVars> : TestHandlerBase<IEntryRepository, EntryEntity, EntryId, THandler>.SetupBase<TVars>
			where TVars : Vars, new()
		{
			public delegate CountEntriesWith<TId> CountEntriesWithAsyncMethod(THandler handler);

			internal async Task Test00(CountEntriesWithAsyncMethod countJourneysWith, Action<ICall, TId> whereId)
			{
				// Arrange
				var (handler, v) = GetVars();
				var userId = LongId<AuthUserId>();
				var entityId = LongId<TId>();
				var count = countJourneysWith(handler);

				// Act
				_ = await count(userId, entityId);

				// Assert
				v.Fluent.AssertCalls(
					c => FluentQueryHelper.AssertWhere<EntryEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
					c => whereId(c, entityId),
					_ => { }
				);
			}

			internal async Task Test01(CountEntriesWithAsyncMethod countJourneysWith)
			{
				// Arrange
				var (handler, v) = GetVars();
				var count = countJourneysWith(handler);

				// Act
				_ = await count(LongId<AuthUserId>(), LongId<TId>());

				// Assert
				await v.Fluent.Received().CountAsync();
			}

			internal async Task Test02(CountEntriesWithAsyncMethod countJourneysWith)
			{
				// Arrange
				var (handler, v) = GetVars();
				var value = Rnd.Lng;
				v.Fluent.CountAsync()
					.Returns(value);
				var count = countJourneysWith(handler);

				// Act
				var result = await count(LongId<AuthUserId>(), LongId<TId>());

				// Assert
				var some = result.AssertSome();
				Assert.Equal(value, some);
			}
		}
	}
}
