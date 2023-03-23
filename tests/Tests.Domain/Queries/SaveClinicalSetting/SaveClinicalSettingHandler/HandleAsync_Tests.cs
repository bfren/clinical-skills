// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.SaveClinicalSetting.Internals;
using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveClinicalSetting.SaveClinicalSettingHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IClinicalSettingRepository, ClinicalSettingEntity, ClinicalSettingId, SaveClinicalSettingHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override SaveClinicalSettingHandler GetHandler(Vars v) =>
				new(v.Repo, v.Dispatcher, v.Log);
		}
	}

	private (SaveClinicalSettingHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new SaveClinicalSettingQuery();
		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(new ClinicalSettingEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Saving Clinical Setting {Query}.", query);
	}

	[Fact]
	public async Task Checks_ClinicalSetting_Belongs_To_User_With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var query = new SaveClinicalSettingQuery { UserId = userId, Id = clinicalSettingId };

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(new ClinicalSettingEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(
			Arg.Is<CheckClinicalSettingBelongsToUserQuery>(x => x.UserId == userId && x.ClinicalSettingId == clinicalSettingId)
		);
	}

	[Fact]
	public async Task ClinicalSetting_Does_Not_Belong_To_User__Returns_None_With_ClinicalSettingDoesNotBelongToUserMsg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new SaveClinicalSettingQuery { Id = LongId<ClinicalSettingId>() };

		v.Dispatcher.SendAsync(Arg.Any<CheckClinicalSettingBelongsToUserQuery>())
			.ReturnsForAnyArgs(false);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertNone().AssertType<Messages.ClinicalSettingDoesNotBelongToUserMsg>();
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var query = new SaveClinicalSettingQuery { UserId = userId, Id = clinicalSettingId };

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(new ClinicalSettingEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<ClinicalSettingEntity, ClinicalSettingId>(c, x => x.Id, Compare.Equal, clinicalSettingId),
			c => FluentQueryHelper.AssertWhere<ClinicalSettingEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_Some__Dispatches_Update__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var version = Rnd.Lng;
		var name = Rnd.Str;
		var query = new SaveClinicalSettingQuery(userId, clinicalSettingId, version, name);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(new ClinicalSettingEntity { Id = clinicalSettingId });

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(
			Arg.Is<UpdateClinicalSettingCommand>(x =>
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
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var query = new SaveClinicalSettingQuery(LongId<AuthUserId>(), LongId<ClinicalSettingId>(), Rnd.Lng, Rnd.Str);
		var updated = Rnd.Flip;

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.SendAsync(Arg.Any<UpdateClinicalSettingCommand>())
			.Returns(updated);
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(new ClinicalSettingEntity { Id = clinicalSettingId });

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(Arg.Any<UpdateClinicalSettingCommand>());
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
		var query = new SaveClinicalSettingQuery(userId, null, 0L, name);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(Create.None<ClinicalSettingEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(
			Arg.Is<CreateClinicalSettingQuery>(c =>
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
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var query = new SaveClinicalSettingQuery(LongId<AuthUserId>(), LongId<ClinicalSettingId>(), Rnd.Lng, Rnd.Str);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.SendAsync(Arg.Any<CreateClinicalSettingQuery>())
			.Returns(clinicalSettingId);
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(Create.None<ClinicalSettingEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(Arg.Any<CreateClinicalSettingQuery>());
		var some = result.AssertSome();
		Assert.Equal(clinicalSettingId, some);
	}
}
