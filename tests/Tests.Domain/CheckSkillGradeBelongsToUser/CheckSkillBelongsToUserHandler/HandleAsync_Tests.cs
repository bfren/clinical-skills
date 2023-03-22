// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Jeebs.Messages;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.CheckSkillBelongsToUser.CheckSkillBelongsToUserHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<ISkillRepository, SkillEntity, SkillId, CheckSkillBelongsToUserHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override CheckSkillBelongsToUserHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);
		}
	}

	private (CheckSkillBelongsToUserHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(new SkillEntity());
		var query = new CheckSkillBelongsToUserQuery(new(), new());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Checking skill {SkillId} belongs to user {UserId}.", query.SkillId.Value, query.UserId.Value);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(new SkillEntity());
		var trainingGradeId = LongId<SkillId>();
		var userId = LongId<AuthUserId>();
		var query = new CheckSkillBelongsToUserQuery(userId, trainingGradeId);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<SkillEntity, SkillId>(c, x => x.Id, Compare.Equal, trainingGradeId),
			c => FluentQueryHelper.AssertWhere<SkillEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__Receives_None__Calls_Log_Msg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var msg = new TestMsg();
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(F.None<SkillEntity>(msg));
		var query = new CheckSkillBelongsToUserQuery(new(), new());

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
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(Create.None<SkillEntity>());
		var query = new CheckSkillBelongsToUserQuery(new(), new());

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
		var entity = new SkillEntity();
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(entity);
		var query = new CheckSkillBelongsToUserQuery(new(), new());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertTrue();
	}

	public sealed record class TestMsg : Msg;
}
