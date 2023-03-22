// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.Repositories;
using ClinicalSkills.Persistence.StrongIds;

namespace ClinicalSkills.Domain.SaveTrainingGrade.Internals.UpdateTrainingGradeHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<ITrainingGradeRepository, TrainingGradeEntity, TrainingGradeId, UpdateTrainingGradeHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override UpdateTrainingGradeHandler GetHandler(Vars v) =>
				new(v.Cache, v.Repo, v.Log);
		}
	}

	private (UpdateTrainingGradeHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		var command = new UpdateTrainingGradeCommand(LongId<TrainingGradeId>(), Rnd.Lng, Rnd.Str);

		// Act
		await handler.HandleAsync(command);

		// Assert
		v.Log.Received().Vrb("Updating Training Grade: {Command}", command);
	}

	[Fact]
	public async Task Calls_Repo_UpdateAsync__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var command = new UpdateTrainingGradeCommand(LongId<TrainingGradeId>(), Rnd.Lng, Rnd.Str);

		// Act
		await handler.HandleAsync(command);

		// Assert
		await v.Repo.Received().UpdateAsync(command);
	}

	[Fact]
	public async void If_Successful__Calls_Cache_RemoveEntry__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var command = new UpdateTrainingGradeCommand(LongId<TrainingGradeId>(), Rnd.Lng, Rnd.Str);
		v.Repo.UpdateAsync(command)
			.Returns(F.True);

		// Act
		await handler.HandleAsync(command);

		// Assert
		v.Cache.Received().RemoveValue(command.Id);
	}

	[Fact]
	public async void Calls_Repo_CreateAsync__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var expected = Rnd.Flip;
		v.Repo.UpdateAsync<UpdateTrainingGradeCommand>(default!)
			.ReturnsForAnyArgs(expected);
		var command = new UpdateTrainingGradeCommand(LongId<TrainingGradeId>(), Rnd.Lng, Rnd.Str);

		// Act
		var result = await handler.HandleAsync(command);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(expected, some);
	}
}
