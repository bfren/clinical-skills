// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.LoadUserSettings.LoadUserSettingsHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IUserSettingsRepository, UserSettingsEntity, UserSettingsId, LoadUserSettingsHandler>
	{
		internal sealed class Setup : SetupBase
		{
			internal override LoadUserSettingsHandler GetHandler(Vars v) =>
				new(v.Log, v.Repo);
		}
	}

	internal (LoadUserSettingsHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<UserSettings>()
			.Returns(new UserSettings(Rnd.Lng, LongId<ClinicalSettingId>(), LongId<TrainingGradeId>()));
		var query = new LoadUserSettingsQuery(LongId<AuthUserId>());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Load settings for User {UserId}", query.Id.Value);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<UserSettings>()
			.Returns(new UserSettings(Rnd.Lng, LongId<ClinicalSettingId>(), LongId<TrainingGradeId>()));
		var userId = LongId<AuthUserId>();
		var query = new LoadUserSettingsQuery(userId);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<UserSettingsEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_QuerySingleAsync__Receives_Some__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var model = new UserSettings(Rnd.Lng, LongId<ClinicalSettingId>(), LongId<TrainingGradeId>());
		v.Fluent.QuerySingleAsync<UserSettings>()
			.Returns(model);
		var query = new LoadUserSettingsQuery(LongId<AuthUserId>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		var some = result.AssertSome();
		Assert.Same(model, some);
	}
	[Fact]
	public async Task Calls_FluentQuery_QuerySingleAsync__Receives_None__Returns_Default_Settings()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<UserSettings>()
			.Returns(Create.None<UserSettings>());
		var query = new LoadUserSettingsQuery(LongId<AuthUserId>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(new(), some);
	}
}
