// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries;
using Jeebs.Auth.Data;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Commands.DeleteClinicalSetting.DeleteClinicalSettingHandler_Tests;

public sealed class HandleAsync_Tests : Abstracts.DeleteOrDisable.HandleAsync_Tests
{
	internal sealed class TestHandler : TestHandlerBase<IClinicalSettingRepository, ClinicalSettingEntity, ClinicalSettingId, DeleteClinicalSettingCommand, DeleteClinicalSettingHandler, ClinicalSettingToDeleteModel, CheckClinicalSettingCanBeDeletedQuery>
	{
		internal sealed class Setup : SetupBase
		{
			public Setup() : base("Clinical Setting") { }

			internal override DeleteClinicalSettingHandler GetHandler(Vars v) =>
				new(v.Cache, v.Repo, v.Dispatcher, v.Log);

			internal override DeleteClinicalSettingCommand GetCommand(AuthUserId? userId = null, ClinicalSettingId? entityId = null)
			{
				if (userId is not null && entityId is not null)
				{
					return new(userId, entityId);
				}

				return new(LongId<AuthUserId>(), LongId<ClinicalSettingId>());
			}

			internal override ClinicalSettingToDeleteModel EmptyModel { get; } =
				new(LongId<ClinicalSettingId>(), Rnd.Lng, Rnd.Flip);
		}
	}

	[Fact]
	public override async Task Test00_Calls_Log_Vrb__With_Query()
	{
		await new TestHandler.Setup().Test00((h, c, d) => h.HandleAsync(c, d));
	}

	[Fact]
	public override async Task Test01_Dispatches_Check_Item_Can_Be_Deleted_Query__With_Correct_Values()
	{
		await new TestHandler.Setup().Test01((h, c, d) => h.HandleAsync(c, d));
	}

	[Fact]
	public override async Task Test02_Calls_Delete_Or_Disable__Receives_Some_True__Removes_Item_From_Cache()
	{
		await new TestHandler.Setup().Test02((h, c, d) => h.HandleAsync(c, d));
	}

	[Fact]
	public override async Task Test03_Calls_Delete_Or_Disable__Receives_Some_False__Leaves_Cache_Alone()
	{
		await new TestHandler.Setup().Test03((h, c, d) => h.HandleAsync(c, d));
	}

	[Fact]
	public override async Task Test04_Calls_Delete_Or_Disable__Receives_None__Leaves_Cache_Alone()
	{
		await new TestHandler.Setup().Test04((h, c, d) => h.HandleAsync(c, d));
	}

	[Fact]
	public override async Task Test05_Calls_Delete_Or_Disable__Returns_Result()
	{
		await new TestHandler.Setup().Test05((h, c, d) => h.HandleAsync(c, d));
	}
}
