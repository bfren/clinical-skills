// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;
using Jeebs.Messages;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.CheckClinicalSettingBelongsToUser.CheckClinicalSettingBelongsToUserHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IClinicalSettingRepository, ClinicalSettingEntity, ClinicalSettingId, CheckClinicalSettingBelongsToUserHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override CheckClinicalSettingBelongsToUserHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);
		}
	}

	private (CheckClinicalSettingBelongsToUserHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(new ClinicalSettingEntity());
		var query = new CheckClinicalSettingBelongsToUserQuery(new(), new());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Checking clinical setting {ClinicalSettingId} belongs to user {UserId}.", query.ClinicalSettingId.Value, query.UserId.Value);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(new ClinicalSettingEntity());
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var userId = LongId<AuthUserId>();
		var query = new CheckClinicalSettingBelongsToUserQuery(userId, clinicalSettingId);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<ClinicalSettingEntity, ClinicalSettingId>(c, x => x.Id, Compare.Equal, clinicalSettingId),
			c => FluentQueryHelper.AssertWhere<ClinicalSettingEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__Receives_None__Calls_Log_Msg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var msg = new TestMsg();
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(F.None<ClinicalSettingEntity>(msg));
		var query = new CheckClinicalSettingBelongsToUserQuery(new(), new());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Msg(msg);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__Receives_None__Returns_False()
	{
		// Arrange
		var (handler, v) = GetVars();
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(Create.None<ClinicalSettingEntity>());
		var query = new CheckClinicalSettingBelongsToUserQuery(new(), new());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertFalse();
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__Receives_Some__Returns_True()
	{
		// Arrange
		var (handler, v) = GetVars();
		var entity = new ClinicalSettingEntity();
		v.Fluent.QuerySingleAsync<ClinicalSettingEntity>()
			.Returns(entity);
		var query = new CheckClinicalSettingBelongsToUserQuery(new(), new());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertTrue();
	}

	public sealed record class TestMsg : Msg;
}
