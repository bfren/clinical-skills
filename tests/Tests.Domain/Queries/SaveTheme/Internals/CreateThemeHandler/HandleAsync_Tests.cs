// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTheme.Internals.CreateThemeHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IThemeRepository, ThemeEntity, ThemeId, CreateThemeHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override CreateThemeHandler GetHandler(Vars v) =>
				new(v.Log, v.Repo);
		}
	}

	private (CreateThemeHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new CreateThemeQuery(new(), Rnd.Str);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Creating Theme: {Query}", query);
	}

	[Fact]
	public async Task Calls_Repo_CreateAsync__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var description = Rnd.Str;
		var query = new CreateThemeQuery(userId, description);

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Repo.Received().CreateAsync(Arg.Is<ThemeEntity>(x =>
			x.UserId == userId
			&& x.Name == description
		));
	}

	[Fact]
	public async void Calls_Repo_CreateAsync__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var expected = LongId<ThemeId>();
		v.Repo.CreateAsync(default!)
			.ReturnsForAnyArgs(expected);
		var query = new CreateThemeQuery(new(), Rnd.Str);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(expected, some);
	}
}
