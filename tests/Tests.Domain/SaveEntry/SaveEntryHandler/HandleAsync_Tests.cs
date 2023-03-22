// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Domain.SaveEntry.Internals;
using ClinicalSkills.Domain.SaveEntry.Messages;
using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.Repositories;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Cryptography.Functions;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;

namespace ClinicalSkills.Domain.SaveEntry.SaveEntryHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IEntryRepository, EntryEntity, EntryId, SaveEntryHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override SaveEntryHandler GetHandler(Vars v) =>
				new(v.Repo, v.Dispatcher, v.Log);
		}
	}

	private (SaveEntryHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new SaveEntryQuery();
		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(new EntryEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Saving Entry {Query}.", query);
	}

	[Fact]
	public async Task Checks_Entry_Belongs_To_User_With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var entryId = LongId<EntryId>();
		var query = new SaveEntryQuery { UserId = userId, Id = entryId };

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(new EntryEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(
			Arg.Is<CheckEntryBelongsToUserQuery>(x => x.UserId == userId && x.EntryId == entryId)
		);
	}

	[Fact]
	public async Task Entry_Does_Not_Belong_To_User__Returns_None_With_EntryDoesNotBelongToUserMsg()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new SaveEntryQuery { Id = LongId<EntryId>() };

		v.Dispatcher.DispatchAsync(Arg.Any<CheckEntryBelongsToUserQuery>())
			.ReturnsForAnyArgs(false);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		result.AssertNone().AssertType<EntryDoesNotBelongToUserMsg>();
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var entryId = LongId<EntryId>();
		var query = new SaveEntryQuery { UserId = userId, Id = entryId };

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(new EntryEntity());

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<EntryEntity, EntryId>(c, x => x.Id, Compare.Equal, entryId),
			c => FluentQueryHelper.AssertWhere<EntryEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { }
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_Some__Dispatches_Update__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var entryId = LongId<EntryId>();
		var version = Rnd.Lng;
		var dateOccurred = Rnd.DateTime;
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var trainingGradeId = LongId<TrainingGradeId>();
		var patientAge = Rnd.Int;
		var caseSummary = CryptoF.Lock(Rnd.Str, Rnd.Str);
		var learningPoints = CryptoF.Lock(Rnd.Str, Rnd.Str);
		var query = new SaveEntryQuery(userId, entryId, version, dateOccurred, clinicalSettingId, trainingGradeId, patientAge, caseSummary, learningPoints);

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(new EntryEntity { Id = entryId });

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(
			Arg.Is<UpdateEntryCommand>(x =>
				x.Id == entryId
				&& x.Version == version
				&& x.DateOccurred == dateOccurred
				&& x.ClinicalSettingId == clinicalSettingId
				&& x.TrainingGradeId == trainingGradeId
				&& x.PatientAge == patientAge
				&& x.CaseSummary == caseSummary
				&& x.LearningPoints == learningPoints
			)
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_Some__Dispatches_Update__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var entryId = LongId<EntryId>();
		var query = new SaveEntryQuery();
		var updated = Rnd.Flip;

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.DispatchAsync(Arg.Any<UpdateEntryCommand>())
			.Returns(updated);
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(new EntryEntity { Id = entryId });

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(Arg.Any<UpdateEntryCommand>());
		var some = result.AssertSome();
		Assert.Equal(entryId, some);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_None__Dispatches_Create__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var dateOccurred = Rnd.DateTime;
		var clinicalSettingId = LongId<ClinicalSettingId>();
		var trainingGradeId = LongId<TrainingGradeId>();
		var patientAge = Rnd.Int;
		var caseSummary = CryptoF.Lock(Rnd.Str, Rnd.Str);
		var learningPoints = CryptoF.Lock(Rnd.Str, Rnd.Str);
		var query = new SaveEntryQuery(userId, null, 0L, dateOccurred, clinicalSettingId, trainingGradeId, patientAge, caseSummary, learningPoints);

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(Create.None<EntryEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(
			Arg.Is<CreateEntryQuery>(x =>
				x.UserId == userId
				&& x.DateOccurred == dateOccurred
				&& x.ClinicalSettingId == clinicalSettingId
				&& x.TrainingGradeId == trainingGradeId
				&& x.PatientAge == patientAge
				&& x.CaseSummary == caseSummary
				&& x.LearningPoints == learningPoints
			)
		);
	}

	[Fact]
	public async Task Checks_Pass__Calls_FluentQuery_QuerySingleAsync__Receives_None__Dispatches_Create__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var entryId = LongId<EntryId>();
		var query = new SaveEntryQuery();

		v.Dispatcher.DispatchAsync<bool>(default!)
			.ReturnsForAnyArgs(true);
		v.Dispatcher.DispatchAsync(Arg.Any<CreateEntryQuery>())
			.Returns(entryId);
		v.Fluent.QuerySingleAsync<EntryEntity>()
			.Returns(Create.None<EntryEntity>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		await v.Dispatcher.Received().DispatchAsync(Arg.Any<CreateEntryQuery>());
		var some = result.AssertSome();
		Assert.Equal(entryId, some);
	}
}
