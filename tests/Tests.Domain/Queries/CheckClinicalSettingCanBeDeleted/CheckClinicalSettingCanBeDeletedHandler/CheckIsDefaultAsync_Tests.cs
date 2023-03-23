// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.CheckClinicalSettingCanBeDeleted.CheckClinicalSettingCanBeDeletedHandler_Tests;

public class CheckIsDefaultAsync_Tests : Abstracts.CheckCanBeDeleted.CheckIsDefaultAsync_Tests
{
	internal sealed class TestHandler : TestHandlerBase<CheckClinicalSettingCanBeDeletedQuery, CheckClinicalSettingCanBeDeletedHandler, ClinicalSettingId>
	{
		internal sealed class Setup : SetupBase
		{
			internal override CheckClinicalSettingCanBeDeletedHandler GetHandler(Vars v) =>
				new(Substitute.For<IEntryRepository>(), v.Log, v.Repo);
		}
	}

	[Fact]
	public override async Task Test00_Calls_FluentQuery_Where__With_Correct_Values()
	{
		await new TestHandler.Setup().Test00(h => h.CheckIsDefaultAsync);
	}

	[Fact]
	public override async Task Test01_Calls_FluentQuery_ExecuteAsync__With_Correct_Values()
	{
		await new TestHandler.Setup().Test01(h => h.CheckIsDefaultAsync, x => x.DefaultClinicalSettingId);
	}

	[Fact]
	public override async Task Test02_Calls_FluentQuery_ExecuteAsync__Receives_Some__Returns_True_When_Ids_Match()
	{
		await new TestHandler.Setup().Test02(h => h.CheckIsDefaultAsync);
	}

	[Fact]
	public override async Task Test03_Calls_FluentQuery_ExecuteAsync__Receives_Some__Returns_False_When_Ids_Do_Not_Match()
	{
		await new TestHandler.Setup().Test03(h => h.CheckIsDefaultAsync);
	}

	[Fact]
	public override async Task Test04_Calls_FluentQuery_ExecuteAsync__Receives_None__Returns_None()
	{
		await new TestHandler.Setup().Test04(h => h.CheckIsDefaultAsync);
	}
}
