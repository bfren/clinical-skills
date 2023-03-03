// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using ClinicalSkills.Domain.CreateUser;
using Jeebs.Cqrs;
using Jeebs.Logging;

namespace ClinicalSkills.Domain.InsertTestData;

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
		var q = from u0 in Dispatcher.DispatchAsync(new CreateUserQuery("bf", "info@bfren.dev", "fred"))
				select true;

		return await q.AuditAsync(
			none: r => Log.Msg(r, LogLevel.Error)
		);
	}
}
