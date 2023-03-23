// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.GetClinicalSetting.GetClinicalSettingHandler_Tests;

public sealed class HandleAsync_Tests : Abstracts.GetSingle.HandleAsync_Tests
{
	internal sealed class TestHandler : TestHandlerBase<IClinicalSettingRepository, ClinicalSettingEntity, ClinicalSettingId, GetClinicalSettingQuery, GetClinicalSettingHandler, ClinicalSettingModel>
	{
		internal sealed class Setup : SetupBase
		{
			internal Setup() : base(true, "Clinical Setting") { }

			internal override GetClinicalSettingHandler GetHandler(Vars v) =>
				new(v.Cache, v.Repo, v.Log);

			internal override GetClinicalSettingQuery GetQuery(AuthUserId? userId = null, ClinicalSettingId? entityId = null)
			{
				if (userId is not null && entityId is not null)
				{
					return new(userId, entityId);
				}

				return new(LongId<AuthUserId>(), LongId<ClinicalSettingId>());
			}

			internal override ClinicalSettingModel NewModel { get; } =
				new(LongId<AuthUserId>(), LongId<ClinicalSettingId>(), Rnd.Lng, Rnd.Str);
		}
	}

	[Fact]
	public override async Task Test00_Id_Is_Null__Returns_None_With_NullMsg()
	{
		await new TestHandler.Setup().Test00<Messages.ClinicalSettingIdIsNullMsg>((h, q) => h.HandleAsync(q));
	}

	[Fact]
	public override async Task Test01_Calls_Log_Vrb__With_Correct_Values()
	{
		await new TestHandler.Setup().Test01((h, q) => h.HandleAsync(q));
	}

	[Fact]
	public override async Task Test02_Calls_FluentQuery_Where__With_Correct_Values()
	{
		await new TestHandler.Setup().Test02((h, q) => h.HandleAsync(q));
	}

	[Fact]
	public override async Task Test03_Calls_FluentQuery_QuerySingleAsync()
	{
		await new TestHandler.Setup().Test03((h, q) => h.HandleAsync(q));
	}

	[Fact]
	public override async Task Test04_Calls_FluentQuery_QuerySingleAsync__Different_UserId__Returns_None_With_DoesNotBelongToUserMsg()
	{
		await new TestHandler.Setup().Test04<Messages.ClinicalSettingDoesNotBelongToUserMsg>((h, q) => h.HandleAsync(q));
	}

	[Fact]
	public override async Task Test05_Calls_FluentQuery_QuerySingleAsync__Same_UserId__Returns_Result()
	{
		await new TestHandler.Setup().Test05((h, q) => h.HandleAsync(q));
	}
}
