// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Commands.SaveUserSettings.Internals;
using Domain.Queries;
using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Commands.SaveUserSettings.SaveUserSettingsHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IUserSettingsRepository, UserSettingsEntity, UserSettingsId, SaveUserSettingsHandler>
	{
		internal sealed class Setup : SetupBase
		{
			internal override SaveUserSettingsHandler GetHandler(Vars v) =>
				new(v.Dispatcher, v.Log, v.Repo);
		}
	}

	internal (SaveUserSettingsHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Checks_ClinicalSetting_Belongs_To_User_With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var settings = new UserSettings(Rnd.Lng, clinicalSettingId, null);
		var query = new SaveUserSettingsCommand(userId, settings);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<UserSettingsEntity>()
			.Returns(new UserSettingsEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(
			Arg.Is<CheckClinicalSettingBelongsToUserQuery>(x => x.UserId == userId && x.ClinicalSettingId == clinicalSettingId)
		);
	}

	[Fact]
	public async Task Checks_TrainingGrade_Belongs_To_User_With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var trainingGradeId = LongId<TrainingGradeId>();
		var settings = new UserSettings(Rnd.Lng, null, trainingGradeId);
		var query = new SaveUserSettingsCommand(userId, settings);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<UserSettingsEntity>()
			.Returns(new UserSettingsEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(
			Arg.Is<CheckTrainingGradeBelongsToUserQuery>(x => x.UserId == userId && x.TrainingGradeId == trainingGradeId)
		);
	}

	[Fact]
	public async Task ClinicalSetting_Does_Not_Belong_To_User__Returns_None_With_SaveUserSettingsCheckFailedMsg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var settings = new UserSettings(Rnd.Lng, LongId<ClinicalSettingId>(), null);
		var query = new SaveUserSettingsCommand(LongId<AuthUserId>(), settings);

		v.Dispatcher.SendAsync(Arg.Any<CheckClinicalSettingBelongsToUserQuery>())
			.ReturnsForAnyArgs(false);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		var msg = result.AssertNone();
		Assert.IsType<Messages.SaveUserSettingsCheckFailedMsg>(msg);
	}

	[Fact]
	public async Task TrainingGrade_Does_Not_Belong_To_User__Returns_None_With_SaveUserSettingsCheckFailedMsg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var settings = new UserSettings(Rnd.Lng, null, LongId<TrainingGradeId>());
		var query = new SaveUserSettingsCommand(LongId<AuthUserId>(), settings);

		v.Dispatcher.SendAsync(Arg.Any<CheckTrainingGradeBelongsToUserQuery>())
			.ReturnsForAnyArgs(false);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		var msg = result.AssertNone();
		Assert.IsType<Messages.SaveUserSettingsCheckFailedMsg>(msg);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var settings = new UserSettings(Rnd.Lng, null, null);
		var query = new SaveUserSettingsCommand(userId, settings);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<UserSettingsEntity>()
			.Returns(new UserSettingsEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<UserSettingsEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_Some__Dispatches_Update__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var settings = new UserSettings(Rnd.Lng, null, null);
		var existingSettings = new UserSettingsEntity();
		var query = new SaveUserSettingsCommand(LongId<AuthUserId>(), settings);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<UserSettingsEntity>()
			.Returns(existingSettings);

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(
			Arg.Is<UpdateUserSettingsCommand>(
				x => x.ExistingSettings == existingSettings && x.UpdatedSettings == settings
			)
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_Some__Dispatches_Update__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var settings = new UserSettings(Rnd.Lng, null, null);
		var query = new SaveUserSettingsCommand(LongId<AuthUserId>(), settings);
		var updated = Rnd.Flip;

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.SendAsync(Arg.Any<UpdateUserSettingsCommand>())
			.Returns(updated);
		v.Fluent.QuerySingleAsync<UserSettingsEntity>()
			.Returns(new UserSettingsEntity());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(Arg.Any<UpdateUserSettingsCommand>());
		var some = result.AssertSome();
		Assert.Equal(updated, some);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_None__Dispatches_Create__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var settings = new UserSettings(Rnd.Lng, null, null);
		var query = new SaveUserSettingsCommand(userId, settings);

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<UserSettingsEntity>()
			.Returns(Create.None<UserSettingsEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(
			Arg.Is<CreateUserSettingsCommand>(
				x => x.UserId == userId && x.Settings == settings
			)
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_None__Dispatches_Create__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var settings = new UserSettings(Rnd.Lng, null, null);
		var query = new SaveUserSettingsCommand(LongId<AuthUserId>(), settings);
		var updated = Rnd.Flip;

		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.SendAsync(Arg.Any<CreateUserSettingsCommand>())
			.Returns(updated);
		v.Fluent.QuerySingleAsync<UserSettingsEntity>()
			.Returns(Create.None<UserSettingsEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().SendAsync(Arg.Any<CreateUserSettingsCommand>());
		var some = result.AssertSome();
		Assert.Equal(updated, some);
	}
}
