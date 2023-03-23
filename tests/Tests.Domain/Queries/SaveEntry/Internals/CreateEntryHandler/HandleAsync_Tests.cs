// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Cryptography.Functions;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.SaveEntry.Internals.CreateEntryHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IEntryRepository, EntryEntity, EntryId, CreateEntryHandler>
	{
		internal sealed class Setup : SetupBase<Vars>
		{
			internal override CreateEntryHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);
		}
	}

	private (CreateEntryHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Calls_Log_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new CreateEntryQuery();

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Creating Entry: {Query}", query);
	}

	[Fact]
	public async Task Calls_Repo_CreateAsync__With_Correct_Values()
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
		var query = new CreateEntryQuery(userId, dateOccurred, clinicalSettingId, trainingGradeId, patientAge, caseSummary, learningPoints);

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Repo.Received().CreateAsync(Arg.Is<EntryEntity>(x =>
			x.UserId == userId
			&& x.DateOccurred == dateOccurred
			&& x.ClinicalSettingId == clinicalSettingId
			&& x.TrainingGradeId == trainingGradeId
			&& x.PatientAge == patientAge
			&& x.CaseSummary == caseSummary
			&& x.LearningPoints == learningPoints
			&& x.Created != DateTime.MinValue
			&& x.LastUpdated != DateTime.MinValue
		));
	}

	[Fact]
	public async void Calls_Repo_CreateAsync__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var expected = LongId<EntryId>();
		v.Repo.CreateAsync(default!)
			.ReturnsForAnyArgs(expected);
		var query = new CreateEntryQuery();

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(expected, some);
	}
}
