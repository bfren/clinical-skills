// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Linq.Expressions;
using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.GetRecentEntries.GetRecentEntriesHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IEntryRepository, EntryEntity, EntryId, GetRecentEntriesHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override GetRecentEntriesHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);
		}
	}

	private (GetRecentEntriesHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Logs_To_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var query = new GetRecentEntriesQuery(userId);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Getting recent entries for user {UserId}.", userId.Value);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var query = new GetRecentEntriesQuery(userId);

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.ExecuteAsync(Arg.Any<Expression<Func<EntryEntity, int?>>>())
			.Returns(Create.None<int?>());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<EntryEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { },
			_ => { },
			_ => { },
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_Sort__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new GetRecentEntriesQuery(LongId<AuthUserId>());

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.ExecuteAsync(Arg.Any<Expression<Func<EntryEntity, int?>>>())
			.Returns(Create.None<int?>());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			_ => { },
			c => FluentQueryHelper.AssertSort<EntryEntity, DateTime>(c, x => x.DateOccurred, SortOrder.Descending),
			c => FluentQueryHelper.AssertSort<EntryEntity, DateTime>(c, x => x.Created, SortOrder.Descending),
			_ => { },
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_Maximum__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new GetRecentEntriesQuery(LongId<AuthUserId>());

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.ExecuteAsync(Arg.Any<Expression<Func<EntryEntity, int?>>>())
			.Returns(Create.None<int?>());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.Received().Maximum(5);
	}
}
