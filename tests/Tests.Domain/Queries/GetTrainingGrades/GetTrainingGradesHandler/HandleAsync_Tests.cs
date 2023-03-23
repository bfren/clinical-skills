// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Data.Enums;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Queries.GetTrainingGrades.GetTrainingGradesHandler_Tests;

public sealed class HandleAsync_Tests : Abstracts.GetEnumerable.HandleAsync_Tests
{
	internal sealed class TestHandler : TestHandlerBase<ITrainingGradeRepository, TrainingGradeEntity, TrainingGradeId, GetTrainingGradesQuery, GetTrainingGradesHandler, TrainingGradesModel>
	{
		internal sealed class Setup : SetupBase
		{
			public Setup() : base("Training Grades") { }

			internal override GetTrainingGradesHandler GetHandler(Vars v) =>
				new(v.Repo, v.Log);

			internal override (GetTrainingGradesQuery, AuthUserId) GetQuery(AuthUserId? userId = null)
			{
				userId ??= LongId<AuthUserId>();
				return (new(userId, Rnd.Flip), userId);
			}

			internal override TrainingGradesModel NewModel { get; } =
				new(LongId<TrainingGradeId>(), Rnd.Str);
		}
	}

	[Fact]
	public override async Task Test00_Id_Is_Null__Returns_None_With_NullMsg()
	{
		await new TestHandler.Setup().Test00<Messages.UserIdIsNullMsg>((h, q) => h.HandleAsync(q));
	}

	[Fact]
	public override async Task Test01_Calls_Log_Vrb__With_Correct_Values()
	{
		await new TestHandler.Setup().Test01((h, q) => h.HandleAsync(q));
	}

	[Fact]
	public override async Task Test02_Calls_FluentQuery_Where__With_Correct_Values()
	{
		await new TestHandler.Setup().Test02_WithIsDisabled((u, d) => new(u, d), (h, q) => h.HandleAsync(q));
	}

	[Fact]
	public override async Task Test03_Calls_FluentQuery_Sort__With_Correct_Values()
	{
		await new TestHandler.Setup().Test03_WithIsDisabled(x => x.Name, SortOrder.Ascending, (h, q) => h.HandleAsync(q));
	}

	[Fact]
	public override async Task Test04_Calls_FluentQuery_QueryAsync()
	{
		await new TestHandler.Setup().Test04((h, q) => h.HandleAsync(q));
	}

	[Fact]
	public override async Task Test05_Calls_FluentQuery_QueryAsync__Returns_Result()
	{
		await new TestHandler.Setup().Test05((h, q) => h.HandleAsync(q));
	}
}
