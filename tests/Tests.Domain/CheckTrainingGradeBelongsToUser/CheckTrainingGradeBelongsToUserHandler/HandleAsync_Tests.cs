// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Jeebs.Messages;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.CheckTrainingGradeBelongsToUser.CheckTrainingGradeBelongsToUserHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<ITrainingGradeRepository, TrainingGradeEntity, TrainingGradeId, CheckTrainingGradeBelongsToUserHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override CheckTrainingGradeBelongsToUserHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);
		}
	}

	private (CheckTrainingGradeBelongsToUserHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(new TrainingGradeEntity());
		var query = new CheckTrainingGradeBelongsToUserQuery(new(), new());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Checking training grade {TrainingGradeId} belongs to user {UserId}.", query.TrainingGradeId.Value, query.UserId.Value);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(new TrainingGradeEntity());
		var trainingGradeId = LongId<TrainingGradeId>();
		var userId = LongId<AuthUserId>();
		var query = new CheckTrainingGradeBelongsToUserQuery(userId, trainingGradeId);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<TrainingGradeEntity, TrainingGradeId>(c, x => x.Id, Compare.Equal, trainingGradeId),
			c => FluentQueryHelper.AssertWhere<TrainingGradeEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__Receives_None__Calls_Log_Msg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var msg = new TestMsg();
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(F.None<TrainingGradeEntity>(msg));
		var query = new CheckTrainingGradeBelongsToUserQuery(new(), new());

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
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(Create.None<TrainingGradeEntity>());
		var query = new CheckTrainingGradeBelongsToUserQuery(new(), new());

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
		var entity = new TrainingGradeEntity();
		v.Fluent.QuerySingleAsync<TrainingGradeEntity>()
			.Returns(entity);
		var query = new CheckTrainingGradeBelongsToUserQuery(new(), new());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertTrue();
	}

	public sealed record class TestMsg : Msg;
}
