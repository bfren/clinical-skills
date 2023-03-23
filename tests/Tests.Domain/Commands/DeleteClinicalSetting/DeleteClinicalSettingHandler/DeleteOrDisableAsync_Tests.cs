// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Commands.DeleteClinicalSetting.DeleteClinicalSettingHandler_Tests;

public class DeleteOrDisableAsync_Tests : Abstracts.DeleteOrDisable.DeleteOrDisableAsync_Tests
{
	internal sealed class TestHandler : TestHandlerBase<IClinicalSettingRepository, ClinicalSettingEntity, ClinicalSettingId, DeleteClinicalSettingCommand, DeleteClinicalSettingHandler, ClinicalSettingToDeleteModel>
	{
		internal sealed class Setup : SetupBase
		{
			internal override DeleteClinicalSettingHandler GetHandler(Vars v) =>
			new(v.Cache, v.Repo, v.Dispatcher, v.Log);

			internal override ClinicalSettingToDeleteModel EmptyModel { get; } =
				new(LongId<ClinicalSettingId>(), Rnd.Lng, Rnd.Flip);
		}
	}

	[Fact]
	public override async Task Test00_Calls_FluentQuery_Where__With_Correct_Values()
	{
		await new TestHandler.Setup().Test00(h => h.DeleteOrDisableAsync);
	}

	[Fact]
	public override async Task Test01_Calls_FluentQuery_WhereSingleAsync__Receives_None__Audits_Msg()
	{
		await new TestHandler.Setup().Test01(h => h.DeleteOrDisableAsync);
	}

	[Fact]
	public override async Task Test02_Calls_FluentQuery_WhereSingleAsync__Receives_None__Returns_None_With_DoesNotExistMsg()
	{
		await new TestHandler.Setup().Test02<Messages.ClinicalSettingDoesNotExistMsg>(h => h.DeleteOrDisableAsync);
	}

	[Fact]
	public override async Task Test03_Is_Delete__Calls_Repo_DeleteAsync__With_Correct_Values()
	{
		await new TestHandler.Setup().Test03((i, v, d) => new ClinicalSettingToDeleteModel(i, v, d), h => h.DeleteOrDisableAsync);
	}

	[Fact]
	public override async Task Test04_Is_Disable__Calls_Repo_UpdateAsync__With_Correct_Values()
	{
		await new TestHandler.Setup().Test04((i, v, d) => new ClinicalSettingToDeleteModel(i, v, d), h => h.DeleteOrDisableAsync);
	}

	[Fact]
	public override async Task Test05_Is_None__Returns_None_With_CannotBeDeletedMsg()
	{
		await new TestHandler.Setup().Test05<Messages.ClinicalSettingCannotBeDeletedMsg>(h => h.DeleteOrDisableAsync);
	}
}
