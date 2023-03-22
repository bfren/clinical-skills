// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Linq.Expressions;
using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.Repositories;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;

namespace ClinicalSkills.Domain.GetEntries.GetEntriesHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IEntryRepository, EntryEntity, EntryId, GetEntriesHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override GetEntriesHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);
		}
	}

	private (GetEntriesHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Logs_To_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var start = DateTime.SpecifyKind(Rnd.DateTime, DateTimeKind.Unspecified);
		var end = DateTime.SpecifyKind(Rnd.DateTime, DateTimeKind.Unspecified);
		var query = new GetEntriesQuery(userId, start, end);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Getting Entries for {User} between {Start} and {End}.", userId.Value, start, end);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var start = DateTime.SpecifyKind(Rnd.DateTime, DateTimeKind.Unspecified);
		var end = DateTime.SpecifyKind(Rnd.DateTime, DateTimeKind.Unspecified);
		var query = new GetEntriesQuery(userId, start, end);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<EntryEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			c => FluentQueryHelper.AssertWhere<EntryEntity, DateTime>(c, x => x.DateOccurred, Compare.MoreThanOrEqual, start),
			c => FluentQueryHelper.AssertWhere<EntryEntity, DateTime>(c, x => x.DateOccurred, Compare.LessThanOrEqual, end),
			_ => { },
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_Sort__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new GetEntriesQuery(LongId<AuthUserId>(), Rnd.DateTime, Rnd.DateTime);

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.ExecuteAsync(Arg.Any<Expression<Func<EntryEntity, int?>>>())
			.Returns(Create.None<int?>());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			_ => { },
			_ => { },
			_ => { },
			c => FluentQueryHelper.AssertSort<EntryEntity, DateTime>(c, x => x.DateOccurred, SortOrder.Descending),
			_ => { }
		);
	}
}
