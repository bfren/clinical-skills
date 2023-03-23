// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Queries;
using Domain.Queries.SaveClinicalSetting.Internals;
using Domain.Queries.SaveEntry.Internals;
using Domain.Queries.SaveTrainingGrade.Internals;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Cryptography.Functions;
using Jeebs.Logging;
using Persistence.StrongIds;
using RndF;

namespace Domain.Commands.InsertTestData;

/// <summary>
/// Insert test data
/// </summary>
internal sealed class InsertTestDataHandler : CommandHandler<InsertTestDataCommand>
{
	private IDispatcher Dispatcher { get; init; }

	private ILog<InsertTestDataHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="dispatcher"></param>
	/// <param name="log"></param>
	public InsertTestDataHandler(IDispatcher dispatcher, ILog<InsertTestDataHandler> log) =>
		(Dispatcher, Log) = (dispatcher, log);

	/// <summary>
	/// Insert various pieces of test data
	/// </summary>
	/// <param name="command"></param>
	public override async Task<Maybe<bool>> HandleAsync(InsertTestDataCommand command)
	{
		Log.Inf("Inserting test data.");
		var pwd = "fred";
		var q = from u0 in Dispatcher.DispatchAsync(new CreateUserQuery("bf", "info@bfren.dev", pwd))
				from ec0 in Dispatcher.DispatchAsync(new GetUserEncryptionKeyQuery(u0, pwd))
				from cs0 in Dispatcher.DispatchAsync(new CreateClinicalSettingQuery(u0, "ED"))
				from cs1 in Dispatcher.DispatchAsync(new CreateClinicalSettingQuery(u0, "MAU"))
				from tg0 in Dispatcher.DispatchAsync(new CreateTrainingGradeQuery(u0, Rnd.Str))
				from tg1 in Dispatcher.DispatchAsync(new CreateTrainingGradeQuery(u0, Rnd.Str))
				from _ in insertEntries(u0, cs0, cs1, tg0, tg1, ec0)
				select true;

		return await q.AuditAsync(
			none: r => Log.Msg(r, LogLevel.Error)
		);

		async Task<Maybe<IEnumerable<EntryId>>> insertEntries(
			AuthUserId userId,
			ClinicalSettingId clinicalSettingId0, ClinicalSettingId clinicalSettingId1,
			TrainingGradeId trainingGradeId0, TrainingGradeId trainingGradeId1,
			string encryptionKey
		)
		{
			Log.Inf("Inserting test entries.");

			var entryIds = new List<EntryId>();
			for (var i = 0; i < 10; i++)
			{
				var clinicalSettingId = Rnd.Flip ? clinicalSettingId0 : clinicalSettingId1;
				var trainingGradeId = Rnd.Flip ? trainingGradeId0 : trainingGradeId1;
				var patientAge = Rnd.NumberF.GetInt32(100);
				var caseSummary = CryptoF.Lock(Rnd.Str, encryptionKey);
				var learningPoints = CryptoF.Lock(Rnd.Str, encryptionKey);

				_ = await Dispatcher
					.DispatchAsync(new CreateEntryQuery(userId, Rnd.DateTime, clinicalSettingId, trainingGradeId, patientAge, caseSummary, learningPoints))
					.IfSomeAsync(entryIds.Add);
			}

			return entryIds;
		}
	}
}
