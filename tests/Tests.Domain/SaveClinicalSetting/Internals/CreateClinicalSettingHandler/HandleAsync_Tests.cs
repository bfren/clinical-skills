// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.SaveClinicalSetting.Internals.CreateClinicalSettingHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IClinicalSettingRepository, ClinicalSettingEntity, ClinicalSettingId, CreateClinicalSettingHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override CreateClinicalSettingHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);
		}
	}

	private (CreateClinicalSettingHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new CreateClinicalSettingQuery(new(), Rnd.Str);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Creating Clinical Setting: {Query}", query);
	}

	[Fact]
	public async Task Calls_Repo_CreateAsync__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var description = Rnd.Str;
		var query = new CreateClinicalSettingQuery(userId, description);

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Repo.Received().CreateAsync(Arg.Is<ClinicalSettingEntity>(x =>
			x.UserId == userId
			&& x.Description == description
		));
	}

	[Fact]
	public async void Calls_Repo_CreateAsync__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var expected = LongId<ClinicalSettingId>();
		v.Repo.CreateAsync(default!)
			.ReturnsForAnyArgs(expected);
		var query = new CreateClinicalSettingQuery(new(), Rnd.Str);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(expected, some);
	}
}
