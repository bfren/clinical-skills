// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.SaveTrainingGrade.Internals.CreateTrainingGradeHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<ITrainingGradeRepository, TrainingGradeEntity, TrainingGradeId, CreateTrainingGradeHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override CreateTrainingGradeHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);
		}
	}

	private (CreateTrainingGradeHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new CreateTrainingGradeQuery(new(), Rnd.Str);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Creating Training Grade: {Query}", query);
	}

	[Fact]
	public async Task Calls_Repo_CreateAsync__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var description = Rnd.Str;
		var query = new CreateTrainingGradeQuery(userId, description);

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Repo.Received().CreateAsync(Arg.Is<TrainingGradeEntity>(x =>
			x.UserId == userId
			&& x.Description == description
		));
	}

	[Fact]
	public async void Calls_Repo_CreateAsync__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var expected = LongId<TrainingGradeId>();
		v.Repo.CreateAsync(default!)
			.ReturnsForAnyArgs(expected);
		var query = new CreateTrainingGradeQuery(new(), Rnd.Str);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(expected, some);
	}
}
