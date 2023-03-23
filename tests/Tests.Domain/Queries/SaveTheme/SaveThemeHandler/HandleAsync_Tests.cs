// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.SaveTheme.Internals;
using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveTheme.SaveThemeHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IThemeRepository, ThemeEntity, ThemeId, SaveThemeHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override SaveThemeHandler GetHandler(Vars v) =>
				new(v.Dispatcher, v.Log, v.Repo);
		}
	}

	private (SaveThemeHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new SaveThemeQuery();
		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(new ThemeEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Saving Theme {Query}.", query);
	}

	[Fact]
	public async Task Checks_Theme_Belongs_To_User_With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<ThemeId>();
		var query = new SaveThemeQuery { UserId = userId, Id = clinicalSettingId };

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(new ThemeEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(
			Arg.Is<CheckThemeBelongsToUserQuery>(x => x.UserId == userId && x.ThemeId == clinicalSettingId)
		);
	}

	[Fact]
	public async Task Theme_Does_Not_Belong_To_User__Returns_None_With_ThemeDoesNotBelongToUserMsg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new SaveThemeQuery { Id = LongId<ThemeId>() };

		v.Dispatcher.DispatchAsync(Arg.Any<CheckThemeBelongsToUserQuery>())
			.ReturnsForAnyArgs(false);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertNone().AssertType<Messages.ThemeDoesNotBelongToUserMsg>();
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<ThemeId>();
		var query = new SaveThemeQuery { UserId = userId, Id = clinicalSettingId };

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(new ThemeEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<ThemeEntity, ThemeId>(c, x => x.Id, Compare.Equal, clinicalSettingId),
			c => FluentQueryHelper.AssertWhere<ThemeEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_Some__Dispatches_Update__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var clinicalSettingId = LongId<ThemeId>();
		var version = Rnd.Lng;
		var name = Rnd.Str;
		var query = new SaveThemeQuery(userId, clinicalSettingId, version, name);

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(new ThemeEntity { Id = clinicalSettingId });

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(
			Arg.Is<UpdateThemeCommand>(x =>
				x.Id == clinicalSettingId
				&& x.Version == version
				&& x.Name == name
			)
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_Some__Dispatches_Update__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var clinicalSettingId = LongId<ThemeId>();
		var query = new SaveThemeQuery(LongId<AuthUserId>(), LongId<ThemeId>(), Rnd.Lng, Rnd.Str);
		var updated = Rnd.Flip;

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.DispatchAsync(Arg.Any<UpdateThemeCommand>())
			.Returns(updated);
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(new ThemeEntity { Id = clinicalSettingId });

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(Arg.Any<UpdateThemeCommand>());
		var some = result.AssertSome();
		Assert.Equal(clinicalSettingId, some);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_None__Dispatches_Create__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var name = Rnd.Str;
		var query = new SaveThemeQuery(userId, null, 0L, name);

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(Create.None<ThemeEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(
			Arg.Is<CreateThemeQuery>(c =>
				c.UserId == userId
				&& c.Name == name
			)
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_None__Dispatches_Create__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var clinicalSettingId = LongId<ThemeId>();
		var query = new SaveThemeQuery(LongId<AuthUserId>(), LongId<ThemeId>(), Rnd.Lng, Rnd.Str);

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.DispatchAsync(Arg.Any<CreateThemeQuery>())
			.Returns(clinicalSettingId);
		v.Fluent.QuerySingleAsync<ThemeEntity>()
			.Returns(Create.None<ThemeEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(Arg.Any<CreateThemeQuery>());
		var some = result.AssertSome();
		Assert.Equal(clinicalSettingId, some);
	}
}
