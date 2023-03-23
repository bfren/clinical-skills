// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Messages;
using Persistence;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;
using StrongId;

namespace Abstracts.CheckCanBeDeleted;

public abstract class HandleAsync_Tests
{
	public abstract Task Test00_Calls_Log_Vrb__With_Correct_Values();

	public abstract Task Test01_Checks_Is_Default__Receives_Some_True__Returns_None_With_IsDefaultMsg();

	public abstract Task Test02_Checks_Is_Default__Receives_None__Returns_None();

	public abstract Task Test03_Counts_Journeys_With__Receives_MoreThan_Zero__Returns_Disable();

	public abstract Task Test04_Counts_Journeys_With__Receives_Zero__Returns_Delete();

	public abstract Task Test05_Counts_Journeys_With__Receives_Negative__Returns_None();

	internal abstract class TestHandlerBase<TQuery, THandler, TId> : TestHandlerBase<IUserSettingsRepository, UserSettingsEntity, UserSettingsId, THandler>
		where TQuery : Query<DeleteOperation>
		where THandler : QueryHandler<TQuery, DeleteOperation>
		where TId : LongId, new()
	{
		internal new abstract class SetupBase : SetupBase<Vars>
		{
			protected SetupBase(string name) : base(name) { }
		}

		internal new abstract class SetupBase<TVars> : TestHandlerBase<IUserSettingsRepository, UserSettingsEntity, UserSettingsId, THandler>.SetupBase
			where TVars : Vars, new()
		{
			internal string Name { get; }

			public delegate Func<TQuery, CheckIsDefault<TId>, CountEntriesWith<TId>, Task<Maybe<DeleteOperation>>> HandleAsyncMethod(THandler handler);

			public delegate CheckIsDefault<TId> CheckIsDefaultAsyncMethod(THandler handler);

			public delegate CountEntriesWith<TId> CountEntriesWithAsyncMethod(THandler handler);

			internal abstract TQuery GetQuery(AuthUserId? userId = null, TId? entityId = null);

			private TQuery NewQuery =>
				GetQuery(LongId<AuthUserId>(), LongId<TId>());

			protected SetupBase(string name) =>
				Name = name;

			internal (THandler handler, CheckIsDefault<TId> check, CountEntriesWith<TId> count, Vars v) GetVars(Maybe<bool>? checkResult = null, long countResult = 0)
			{
				var (handler, v) = base.GetVars();
				var check = Substitute.For<CheckIsDefault<TId>>();
				check.Invoke(default!, default!)
					.ReturnsForAnyArgs(checkResult ?? Create.None<bool>());
				var count = Substitute.For<CountEntriesWith<TId>>();
				count.Invoke(default!, default!)
					.ReturnsForAnyArgs(countResult);

				return (handler, check, count, v);
			}

			public async Task Test00(HandleAsyncMethod handleAsyncMethod)
			{
				// Arrange
				var (handler, check, count, v) = GetVars();
				var entityId = LongId<TId>();
				var query = GetQuery(LongId<AuthUserId>(), entityId);
				var handle = handleAsyncMethod(handler);

				// Act
				_ = await handle(query, check, count);

				// Assert
				v.Log.Received().Vrb($"Checking whether or not {Name} {{{Name.Replace(" ", "")}Id}} can be deleted.", entityId.Value);
			}

			public async Task Test01<TIsDefaultMsg>(HandleAsyncMethod handleAsyncMethod)
				where TIsDefaultMsg : Msg
			{
				// Arrange
				var (handler, check, count, v) = GetVars(checkResult: F.True);
				var query = NewQuery;
				var handle = handleAsyncMethod(handler);

				// Act
				var result = await handle(query, check, count);

				// Assert
				result.AssertNone().AssertType<TIsDefaultMsg>();
			}

			public async Task Test02(HandleAsyncMethod handleAsyncMethod)
			{
				// Arrange
				var (handler, check, count, v) = GetVars();
				var query = NewQuery;
				var handle = handleAsyncMethod(handler);

				// Act
				var result = await handle(query, check, count);

				// Assert
				result.AssertNone();
			}

			public async Task Test03(HandleAsyncMethod handleAsyncMethod)
			{
				// Arrange
				var (handler, check, count, v) = GetVars(checkResult: F.False, countResult: Rnd.Lng);
				var query = NewQuery;
				var handle = handleAsyncMethod(handler);

				// Act
				var result = await handle(query, check, count);

				// Assert
				var some = result.AssertSome();
				Assert.Equal(DeleteOperation.Disable, some);
			}

			public async Task Test04(HandleAsyncMethod handleAsyncMethod)
			{
				// Arrange
				var (handler, check, count, v) = GetVars(checkResult: F.False, countResult: 0);
				var query = NewQuery;
				var handle = handleAsyncMethod(handler);

				// Act
				var result = await handle(query, check, count);

				// Assert
				var some = result.AssertSome();
				Assert.Equal(DeleteOperation.Delete, some);
			}

			public async Task Test05(HandleAsyncMethod handleAsyncMethod)
			{
				// Arrange
				var (handler, check, count, v) = GetVars(checkResult: F.False, countResult: Rnd.Lng * -1);
				var query = NewQuery;
				var handle = handleAsyncMethod(handler);

				// Act
				var result = await handle(query, check, count);

				// Assert
				var some = result.AssertSome();
				Assert.Equal(DeleteOperation.None, some);
			}
		}
	}
}
