// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Jeebs.Messages;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.CheckEntryBelongsToUser.CheckEntryBelongsToUserHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IEntryRepository, EntryEntity, EntryId, CheckEntryBelongsToUserHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override CheckEntryBelongsToUserHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);
		}
	}

	private (CheckEntryBelongsToUserHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(new EntryEntity());
		var query = new CheckEntryBelongsToUserQuery(new(), new());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Checking entry {EntryId} belongs to user {UserId}.", query.EntryId.Value, query.UserId.Value);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(new EntryEntity());
		var entryId = LongId<EntryId>();
		var userId = LongId<AuthUserId>();
		var query = new CheckEntryBelongsToUserQuery(userId, entryId);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<EntryEntity, EntryId>(c, x => x.Id, Compare.Equal, entryId),
			c => FluentQueryHelper.AssertWhere<EntryEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__Receives_None__Calls_Log_Msg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var msg = new TestMsg();
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(F.None<EntryEntity>(msg));
		var query = new CheckEntryBelongsToUserQuery(new(), new());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Msg(msg);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__Receives_None__Returns_False()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(Create.None<EntryEntity>());
		var query = new CheckEntryBelongsToUserQuery(new(), new());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertFalse();
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__Receives_Some__Returns_True()
	{
		// Arrange
		var (handler, v) = GetVars();
		var entity = new EntryEntity();
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(entity);
		var query = new CheckEntryBelongsToUserQuery(new(), new());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertTrue();
	}

	public sealed record class TestMsg : Msg;
}
