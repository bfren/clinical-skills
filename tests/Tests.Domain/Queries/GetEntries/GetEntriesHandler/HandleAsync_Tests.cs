// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Linq.Expressions;
using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.GetEntries.GetEntriesHandler_Tests;

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
		var query = new GetEntriesQuery(userId);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Getting Entries for {User} matching {Query}.", userId.Value, query);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var occurredFrom = DateTime.SpecifyKind(Rnd.DateTime, DateTimeKind.Unspecified);
		var occurredTo = DateTime.SpecifyKind(Rnd.DateTime, DateTimeKind.Unspecified);
		var ageFrom = Rnd.Int;
		var ageTo = Rnd.Int;
		var query = new GetEntriesQuery(userId)
		{
			DateOccurredFrom = occurredFrom,
			DateOccurredTo = occurredTo,
			PatientAgeFrom = ageFrom,
			PatientAgeTo = ageTo
		};

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<EntryEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			c => FluentQueryHelper.AssertWhere<EntryEntity, DateTime>(c, x => x.DateOccurred, Compare.MoreThanOrEqual, occurredFrom),
			c => FluentQueryHelper.AssertWhere<EntryEntity, DateTime>(c, x => x.DateOccurred, Compare.LessThanOrEqual, occurredTo),
			c => FluentQueryHelper.AssertWhere<EntryEntity, int>(c, x => x.PatientAge, Compare.MoreThanOrEqual, ageFrom),
			c => FluentQueryHelper.AssertWhere<EntryEntity, int>(c, x => x.PatientAge, Compare.LessThanOrEqual, ageTo),
			_ => { },
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_Sort__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new GetEntriesQuery(LongId<AuthUserId>());

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.ExecuteAsync(Arg.Any<Expression<Func<EntryEntity, int?>>>())
			.Returns(Create.None<int?>());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			_ => { },
			c => FluentQueryHelper.AssertSort<EntryEntity, DateTime>(c, x => x.DateOccurred, SortOrder.Descending),
			_ => { }
		);
	}
}
