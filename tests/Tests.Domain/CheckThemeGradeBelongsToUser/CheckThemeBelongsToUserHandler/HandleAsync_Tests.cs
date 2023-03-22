// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Jeebs.Messages;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.CheckThemeBelongsToUser.CheckThemeBelongsToUserHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IThemeRepository, ThemeEntity, ThemeId, CheckThemeBelongsToUserHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override CheckThemeBelongsToUserHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);
		}
	}

	private (CheckThemeBelongsToUserHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(new ThemeEntity());
		var query = new CheckThemeBelongsToUserQuery(new(), new());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Checking theme {ThemeId} belongs to user {UserId}.", query.ThemeId.Value, query.UserId.Value);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(new ThemeEntity());
		var trainingGradeId = LongId<ThemeId>();
		var userId = LongId<AuthUserId>();
		var query = new CheckThemeBelongsToUserQuery(userId, trainingGradeId);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<ThemeEntity, ThemeId>(c, x => x.Id, Compare.Equal, trainingGradeId),
			c => FluentQueryHelper.AssertWhere<ThemeEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__Receives_None__Calls_Log_Msg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var msg = new TestMsg();
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(F.None<ThemeEntity>(msg));
		var query = new CheckThemeBelongsToUserQuery(new(), new());

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
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(Create.None<ThemeEntity>());
		var query = new CheckThemeBelongsToUserQuery(new(), new());

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
		var entity = new ThemeEntity();
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(entity);
		var query = new CheckThemeBelongsToUserQuery(new(), new());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertTrue();
	}

	public sealed record class TestMsg : Msg;
}
