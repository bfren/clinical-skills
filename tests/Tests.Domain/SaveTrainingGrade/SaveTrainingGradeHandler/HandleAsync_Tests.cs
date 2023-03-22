// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.SaveTrainingGrade.Internals;
using Domain.SaveTrainingGrade.Messages;
using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.SaveTrainingGrade.SaveTrainingGradeHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<ITrainingGradeRepository, TrainingGradeEntity, TrainingGradeId, SaveTrainingGradeHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override SaveTrainingGradeHandler GetHandler(Vars v) =>
				new(v.Repo, v.Dispatcher, v.Log);
		}
	}

	private (SaveTrainingGradeHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new SaveTrainingGradeQuery();
		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(new TrainingGradeEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Saving Training Grade {Query}.", query);
	}

	[Fact]
	public async Task Checks_TrainingGrade_Belongs_To_User_With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<TrainingGradeId>();
		var query = new SaveTrainingGradeQuery { UserId = userId, Id = clinicalSettingId };

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(new TrainingGradeEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(
			Arg.Is<CheckTrainingGradeBelongsToUserQuery>(x => x.UserId == userId && x.TrainingGradeId == clinicalSettingId)
		);
	}

	[Fact]
	public async Task TrainingGrade_Does_Not_Belong_To_User__Returns_None_With_TrainingGradeDoesNotBelongToUserMsg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new SaveTrainingGradeQuery { Id = LongId<TrainingGradeId>() };

		v.Dispatcher.DispatchAsync(Arg.Any<CheckTrainingGradeBelongsToUserQuery>())
			.ReturnsForAnyArgs(false);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertNone().AssertType<TrainingGradeDoesNotBelongToUserMsg>();
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<TrainingGradeId>();
		var query = new SaveTrainingGradeQuery { UserId = userId, Id = clinicalSettingId };

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(new TrainingGradeEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<TrainingGradeEntity, TrainingGradeId>(c, x => x.Id, Compare.Equal, clinicalSettingId),
			c => FluentQueryHelper.AssertWhere<TrainingGradeEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_Some__Dispatches_Update__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<TrainingGradeId>();
		var version = Rnd.Lng;
		var name = Rnd.Str;
		var query = new SaveTrainingGradeQuery(userId, clinicalSettingId, version, name);

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(new TrainingGradeEntity { Id = clinicalSettingId });

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(
			Arg.Is<UpdateTrainingGradeCommand>(x =>
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
		var clinicalSettingId = LongId<TrainingGradeId>();
		var query = new SaveTrainingGradeQuery(LongId<AuthUserId>(), LongId<TrainingGradeId>(), Rnd.Lng, Rnd.Str);
		var updated = Rnd.Flip;

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.DispatchAsync(Arg.Any<UpdateTrainingGradeCommand>())
			.Returns(updated);
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(new TrainingGradeEntity { Id = clinicalSettingId });

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(Arg.Any<UpdateTrainingGradeCommand>());
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
		var query = new SaveTrainingGradeQuery(userId, null, 0L, name);

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(Create.None<TrainingGradeEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(
			Arg.Is<CreateTrainingGradeQuery>(c =>
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
		var clinicalSettingId = LongId<TrainingGradeId>();
		var query = new SaveTrainingGradeQuery(LongId<AuthUserId>(), LongId<TrainingGradeId>(), Rnd.Lng, Rnd.Str);

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.DispatchAsync(Arg.Any<CreateTrainingGradeQuery>())
			.Returns(clinicalSettingId);
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(Create.None<TrainingGradeEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(Arg.Any<CreateTrainingGradeQuery>());
		var some = result.AssertSome();
		Assert.Equal(clinicalSettingId, some);
	}
}
