// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Data;
using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Data;
using Jeebs.Logging;
using Persistence;
using Persistence.Tables;

namespace Domain.TruncateEverything;

/// <summary>
/// Truncate all tables
/// </summary>
internal sealed class TruncateEverythingHandler : CommandHandler<TruncateEverythingCommand>
{
	private IDb Db { get; init; }

	private ILog<TruncateEverythingHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="db"></param>
	/// <param name="log"></param>
	public TruncateEverythingHandler(IDb db, ILog<TruncateEverythingHandler> log) =>
		(Db, Log) = (db, log);

	/// <summary>
	/// Truncate all tables
	/// </summary>
	/// <param name="command"></param>
	public override async Task<Maybe<bool>> HandleAsync(TruncateEverythingCommand command)
	{
		Log.Inf("Truncating all database tables.");

		Task truncate(string schema, string table, IDbTransaction transaction)
		{
			Log.Dbg("Truncating table {Schema}.{Table}.", schema, table);
			return Db.ExecuteAsync($"TRUNCATE TABLE {schema}.{table};", null, CommandType.Text, transaction);
		}

		using var w = await Db.StartWorkAsync();
		await truncate("auth", "user", w.Transaction);
		await truncate(Constants.Schema, ClinicalSettingTable.TableName, w.Transaction);
		await truncate(Constants.Schema, EntrySkillTable.TableName, w.Transaction);
		await truncate(Constants.Schema, EntryTable.TableName, w.Transaction);
		await truncate(Constants.Schema, EntryThemeTable.TableName, w.Transaction);
		await truncate(Constants.Schema, SkillTable.TableName, w.Transaction);
		await truncate(Constants.Schema, ThemeTable.TableName, w.Transaction);
		await truncate(Constants.Schema, TrainingGradeTable.TableName, w.Transaction);
		await truncate(Constants.Schema, UserEncryptionTable.TableName, w.Transaction);

		return true;
	}
}
