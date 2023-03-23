// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Messages;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Commands.SaveUserSettings.Internals.CreateSettingsHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IUserSettingsRepository, UserSettingsEntity, UserSettingsId, CreateUserSettingsHandler>
	{
		internal sealed class Setup : SetupBase
		{
			internal override CreateUserSettingsHandler GetHandler(Vars v) =>
				new(v.Log, v.Repo);
		}
	}

	internal (CreateUserSettingsHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Logs_Vrb__With_UserId()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var command = new CreateUserSettingsCommand(userId, new());
		v.Repo.CreateAsync(default!)
			.ReturnsForAnyArgs(Create.None<UserSettingsId>());

		// Act
		await handler.HandleAsync(command);

		// Assert
		v.Log.Received().Vrb("Creating new settings for user {UserId}.", userId.Value);
	}

	[Fact]
	public async Task Calls_Repo_CreateAsync__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var trainingGradeId = LongId<TrainingGradeId>();
		var command = new CreateUserSettingsCommand(userId, new(new(), 0L, clinicalSettingId, trainingGradeId));
		v.Repo.CreateAsync(default!)
			.ReturnsForAnyArgs(LongId<UserSettingsId>());

		// Act
		await handler.HandleAsync(command);

		// Assert
		await v.Repo.Received().CreateAsync(Arg.Is<UserSettingsEntity>(
			x => x.UserId == userId && x.DefaultClinicalSettingId == clinicalSettingId && x.DefaultTrainingGradeId == trainingGradeId
		));
	}

	[Fact]
	public async Task Calls_Repo_CreateAsync__Receives_Some__Audits_To_Vrb()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var settingsId = LongId<UserSettingsId>();
		var command = new CreateUserSettingsCommand(userId, new());
		v.Repo.CreateAsync(default!)
			.ReturnsForAnyArgs(settingsId);

		// Act
		await handler.HandleAsync(command);

		// Assert
		v.Log.Received().Vrb("Created settings {SettingsId} for user {UserId}.", settingsId.Value, userId.Value);
	}

	[Fact]
	public async Task Calls_Repo_CreateAsync__Receives_Some__Returns_True()
	{
		// Arrange
		var (handler, v) = GetVars();
		var command = new CreateUserSettingsCommand(new(), new());
		v.Repo.CreateAsync(default!)
			.ReturnsForAnyArgs(LongId<UserSettingsId>());

		// Act
		var result = await handler.HandleAsync(command);

		// Assert
		result.AssertTrue();
	}

	[Fact]
	public async void Calls_Repo_CreateAsync__Receives_None__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var command = new CreateUserSettingsCommand(new(), new());
		var msg = new TestMsg();
		var failed = F.None<UserSettingsId>(msg);
		v.Repo.CreateAsync(default!)
			.ReturnsForAnyArgs(failed);

		// Act
		var result = await handler.HandleAsync(command);

		// Assert
		var none = result.AssertNone();
		Assert.Same(msg, none);
	}

	public sealed record class TestMsg : Msg;
}
