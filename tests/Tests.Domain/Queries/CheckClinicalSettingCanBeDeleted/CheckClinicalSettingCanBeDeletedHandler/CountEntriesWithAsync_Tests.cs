// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.CheckClinicalSettingCanBeDeleted.CheckClinicalSettingCanBeDeletedHandler_Tests;

public class CountEntriesWithAsync_Tests : Abstracts.CheckCanBeDeleted.CountEntriesWithAsync_Tests
{
	internal sealed class TestHandler : TestHandlerBase<CheckClinicalSettingCanBeDeletedQuery, CheckClinicalSettingCanBeDeletedHandler, ClinicalSettingId>
	{
		internal sealed class Setup : SetupBase
		{
			internal override CheckClinicalSettingCanBeDeletedHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log, Substitute.For<IUserSettingsRepository>());
		}
	}

	[Fact]
	public override async Task Test00_Calls_FluentQuery_Where__With_Correct_Values()
	{
		await new TestHandler.Setup().Test00(
			h => h.CountEntriesWithAsync,
			(c, id) => FluentQueryHelper.AssertWhere<EntryEntity, ClinicalSettingId?>(c, x => x.ClinicalSettingId, Compare.Equal, id)
		);
	}

	[Fact]
	public override async Task Test01_Calls_FluentQuery_CountAsync()
	{
		await new TestHandler.Setup().Test01(h => h.CountEntriesWithAsync);
	}

	[Fact]
	public override async Task Test02_Calls_FluentQuery_CountAsync__Returns_Result()
	{
		await new TestHandler.Setup().Test02(h => h.CountEntriesWithAsync);
	}
}
