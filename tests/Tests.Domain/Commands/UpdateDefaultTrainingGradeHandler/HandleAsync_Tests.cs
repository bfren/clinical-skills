// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Persistence.StrongIds;

namespace Domain.Commands.UpdateDefaultTrainingGrade.UpdateDefaultTrainingGradeHandler_Tests;

public sealed class HandleAsync_Tests : Abstracts.UpdateSettings.HandleAsync_Tests
{
	internal sealed class TestHandler : TestHandlerBase<UpdateDefaultTrainingGradeCommand, UpdateDefaultTrainingGradeHandler, TrainingGradeId>
	{
		internal sealed class Setup : SetupBase
		{
			public Setup() : base("Training Grade") { }

			internal override UpdateDefaultTrainingGradeCommand GetCommand(AuthUserId? userId = null, TrainingGradeId? itemId = null) =>
				new(userId ?? LongId<AuthUserId>(), LongId<UserSettingsId>(), Rnd.Lng, itemId);

			internal override UpdateDefaultTrainingGradeHandler GetHandler(Vars v) =>
				new(v.Dispatcher, v.Log, v.Repo);
		}
	}

	[Fact]
	public override async Task Test00_ItemId_Is_Not_Null__Checks_Item_Belongs_To_User__Receives_None__Returns_None_With_SaveUserSettingsCheckFailedMsg()
	{
		await new TestHandler.Setup().Test00<Messages.SaveSettingsCheckFailedMsg>((h, c) => h.HandleAsync(c));
	}

	[Fact]
	public override async Task Test01_ItemId_Is_Not_Null__Checks_Item_Belongs_To_User__Receives_Some_False__Returns_None_With_SaveUserSettingsCheckFailedMsg()
	{
		await new TestHandler.Setup().Test01<Messages.SaveSettingsCheckFailedMsg>((h, c) => h.HandleAsync(c));
	}

	[Fact]
	public override async Task Test02_ItemId_Is_Not_Null__Checks_Item_Belongs_To_User__Receives_Some_True__Calls_Log_Vrb__With_Correct_Values()
	{
		await new TestHandler.Setup().Test02((h, c) => h.HandleAsync(c));
	}

	[Fact]
	public override async Task Test03_ItemId_Is_Not_Null__Checks_Item_Belongs_To_User__Receives_Some_True__Calls_UserSettings_UpdateAsync__With_Correct_Values()
	{
		await new TestHandler.Setup().Test03((h, c) => h.HandleAsync(c));
	}

	[Fact]
	public override async Task Test04_ItemId_Is_Null__Calls_Log_Vrb__With_Correct_Values()
	{
		await new TestHandler.Setup().Test04((h, c) => h.HandleAsync(c));
	}

	[Fact]
	public override async Task Test05_ItemId_Is_Null__Calls_UserSettings_UpdateAsync__With_Correct_Values()
	{
		await new TestHandler.Setup().Test05((h, c) => h.HandleAsync(c));
	}
}
