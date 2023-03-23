// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Commands.SaveUserSettings.SaveUserSettingsHandler_Tests;

public class CheckClinicalSettingBelongsToUser_Tests
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
	public async Task With_ClinicalSettingId__Calls_Dispatcher_SendAsync__Receives_Some__Returns_Value()
	{
		// Arrange
		var (handler, v) = GetVars();
		var value = Rnd.Flip;
		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(F.Some(value).AsTask());

		// Act
		var result = await handler.CheckClinicalSettingBelongsToUser(LongId<ClinicalSettingId>(), LongId<AuthUserId>());

		// Assert
		Assert.Equal(value, result);
	}

	[Fact]
	public async Task With_ClinicalSettingId__Calls_Dispatcher_SendAsync__Receives_None__Returns_False()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Dispatcher.SendAsync<bool>(default!)
			.ReturnsForAnyArgs(Create.None<bool>());

		// Act
		var result = await handler.CheckClinicalSettingBelongsToUser(LongId<ClinicalSettingId>(), LongId<AuthUserId>());

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task Without_ClinicalSettingId__Returns_True()
	{
		// Arrange
		var (handler, _) = GetVars();

		// Act
		var result = await handler.CheckClinicalSettingBelongsToUser(null, new());

		// Assert
		Assert.True(result);
	}
}
