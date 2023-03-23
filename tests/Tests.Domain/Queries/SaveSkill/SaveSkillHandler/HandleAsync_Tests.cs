// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.SaveSkill.Internals;
using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveSkill.SaveSkillHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<ISkillRepository, SkillEntity, SkillId, SaveSkillHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override SaveSkillHandler GetHandler(Vars v) =>
				new(v.Dispatcher, v.Log, v.Repo);
		}
	}

	private (SaveSkillHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new SaveSkillQuery();
		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(new SkillEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Saving Skill {Query}.", query);
	}

	[Fact]
	public async Task Checks_Skill_Belongs_To_User_With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<SkillId>();
		var query = new SaveSkillQuery { UserId = userId, Id = clinicalSettingId };

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(new SkillEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(
			Arg.Is<CheckSkillBelongsToUserQuery>(x => x.UserId == userId && x.SkillId == clinicalSettingId)
		);
	}

	[Fact]
	public async Task Skill_Does_Not_Belong_To_User__Returns_None_With_SkillDoesNotBelongToUserMsg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new SaveSkillQuery { Id = LongId<SkillId>() };

		v.Dispatcher.SendAsync(Arg.Any<CheckSkillBelongsToUserQuery>())
			.ReturnsForAnyArgs(false);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertNone().AssertType<Messages.SkillDoesNotBelongToUserMsg>();
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<SkillId>();
		var query = new SaveSkillQuery { UserId = userId, Id = clinicalSettingId };

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(new SkillEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<SkillEntity, SkillId>(c, x => x.Id, Compare.Equal, clinicalSettingId),
			c => FluentQueryHelper.AssertWhere<SkillEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_Some__Dispatches_Update__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<SkillId>();
		var version = Rnd.Lng;
		var name = Rnd.Str;
		var query = new SaveSkillQuery(userId, clinicalSettingId, version, name);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(new SkillEntity { Id = clinicalSettingId });

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(
			Arg.Is<UpdateSkillCommand>(x =>
				x.Id == clinicalSettingId
				&& x.Version == version
				&& x.Name == name
			)
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_Some__Dispatches_Update__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var clinicalSettingId = LongId<SkillId>();
		var query = new SaveSkillQuery(LongId<AuthUserId>(), LongId<SkillId>(), Rnd.Lng, Rnd.Str);
		var updated = Rnd.Flip;

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.SendAsync(Arg.Any<UpdateSkillCommand>())
			.Returns(updated);
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(new SkillEntity { Id = clinicalSettingId });

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(Arg.Any<UpdateSkillCommand>());
		var some = result.AssertSome();
		Assert.Equal(clinicalSettingId, some);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_None__Dispatches_Create__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var name = Rnd.Str;
		var query = new SaveSkillQuery(userId, null, 0L, name);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(Create.None<SkillEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(
			Arg.Is<CreateSkillQuery>(c =>
				c.UserId == userId
				&& c.Name == name
			)
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_None__Dispatches_Create__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var clinicalSettingId = LongId<SkillId>();
		var query = new SaveSkillQuery(LongId<AuthUserId>(), LongId<SkillId>(), Rnd.Lng, Rnd.Str);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.SendAsync(Arg.Any<CreateSkillQuery>())
			.Returns(clinicalSettingId);
		v.Fluent.QuerySingleAsync<SkillEntity>()
			.Returns(Create.None<SkillEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(Arg.Any<CreateSkillQuery>());
		var some = result.AssertSome();
		Assert.Equal(clinicalSettingId, some);
	}
}
