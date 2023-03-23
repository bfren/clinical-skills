// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.CheckClinicalSettingCanBeDeleted.CheckClinicalSettingCanBeDeletedHandler_Tests;

public sealed class HandleAsync_Tests : Abstracts.CheckCanBeDeleted.HandleAsync_Tests
{
	internal sealed class TestHandler : TestHandlerBase<CheckClinicalSettingCanBeDeletedQuery, CheckClinicalSettingCanBeDeletedHandler, ClinicalSettingId>
	{
		internal sealed class Setup : SetupBase
		{
			public Setup() : base("Clinical Setting") { }

			internal override CheckClinicalSettingCanBeDeletedHandler GetHandler(Vars v) =>
				new(Substitute.For<IEntryRepository>(), v.Log, v.Repo);

			internal override CheckClinicalSettingCanBeDeletedQuery GetQuery(AuthUserId? userId = null, ClinicalSettingId? entityId = null)
			{
				if (userId is not null && entityId is not null)
				{
					return new(userId, entityId);
				}

				return new(LongId<AuthUserId>(), LongId<ClinicalSettingId>());
			}
		}
	}

	[Fact]
	public override async Task Test00_Calls_Log_Vrb__With_Correct_Values()
	{
		await new TestHandler.Setup().Test00(h => h.HandleAsync);
	}

	[Fact]
	public override async Task Test01_Checks_Is_Default__Receives_Some_True__Returns_None_With_IsDefaultMsg()
	{
		await new TestHandler.Setup().Test01<Messages.ClinicalSettingIsDefaultClinicalSettingMsg>(h => h.HandleAsync);
	}

	[Fact]
	public override async Task Test02_Checks_Is_Default__Receives_None__Returns_None()
	{
		await new TestHandler.Setup().Test02(h => h.HandleAsync);
	}

	[Fact]
	public override async Task Test03_Counts_Journeys_With__Receives_MoreThan_Zero__Returns_Disable()
	{
		await new TestHandler.Setup().Test03(h => h.HandleAsync);
	}

	[Fact]
	public override async Task Test04_Counts_Journeys_With__Receives_Zero__Returns_Delete()
	{
		await new TestHandler.Setup().Test04(h => h.HandleAsync);
	}

	[Fact]
	public override async Task Test05_Counts_Journeys_With__Receives_Negative__Returns_None()
	{
		await new TestHandler.Setup().Test05(h => h.HandleAsync);
	}
}
