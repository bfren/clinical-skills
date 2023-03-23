// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Commands.SaveUserSettings.Internals.UpdateSettingsHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IUserSettingsRepository, UserSettingsEntity, UserSettingsId, UpdateUserSettingsHandler>
	{
		internal sealed class Setup : SetupBase
		{
			internal override UpdateUserSettingsHandler GetHandler(Vars v) =>
				new(v.Log, v.Repo);
		}
	}

	internal (UpdateUserSettingsHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Logs_Vrb__With_UserId()
	{
		// Arrange
		var (handler, v) = GetVars();
		var settingsId = LongId<UserSettingsId>();
		var userId = LongId<AuthUserId>();
		var command = new UpdateUserSettingsCommand(new() { Id = settingsId, UserId = userId }, new());
		v.Repo.UpdateAsync<UserSettingsEntity>(default!)
			.ReturnsForAnyArgs(false);

		// Act
		await handler.HandleAsync(command);

		// Assert
		v.Log.Received().Vrb("Updating settings {SettingsId} for user {UserId}.", settingsId.Value, userId.Value);
	}

	[Fact]
	public async Task Calls_Repo_CreateAsync__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var settingsId = LongId<UserSettingsId>();
		var version = Rnd.Lng;
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var trainingGradeId = LongId<TrainingGradeId>();
		var existingSettings = new UserSettingsEntity
		{
			Id = settingsId,
			Version = Rnd.Lng,
			UserId = userId,
			DefaultClinicalSettingId = LongId<ClinicalSettingId>(),
			DefaultTrainingGradeId = LongId<TrainingGradeId>()
		};
		var updatedSettings = new UserSettings
		{
			Version = version,
			DefaultClinicalSettingId = clinicalSettingId,
			DefaultTrainingGradeId = trainingGradeId
		};
		var command = new UpdateUserSettingsCommand(existingSettings, updatedSettings);
		v.Repo.UpdateAsync<UserSettingsEntity>(default!)
			.ReturnsForAnyArgs(false);

		// Act
		await handler.HandleAsync(command);

		// Assert
		await v.Repo.Received().UpdateAsync(Arg.Is<UserSettingsEntity>(x =>
			x.Id == settingsId
			&& x.Version == version
			&& x.UserId == userId
			&& x.DefaultClinicalSettingId == clinicalSettingId
			&& x.DefaultTrainingGradeId == trainingGradeId
		));
	}

	[Fact]
	public async Task Calls_Repo_CreateAsync__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var value = Rnd.Flip;
		var command = new UpdateUserSettingsCommand(new(), new());
		v.Repo.UpdateAsync<UserSettingsEntity>(default!)
			.ReturnsForAnyArgs(value);

		// Act
		var result = await handler.HandleAsync(command);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(value, some);
	}
}
