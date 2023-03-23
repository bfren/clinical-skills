// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Threading.Tasks;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Jeebs.Messages;
using Persistence.Repositories;

namespace Domain.Queries.GetUserEncryptionKey;

/// <summary>
/// Retrieve a user's encryption key from the database
/// </summary>
public sealed class GetUserEncryptionKeyHandler : QueryHandler<GetUserEncryptionKeyQuery, string>
{
	private ILog Log { get; init; }

	private IUserEncryptionRepository UserEncryption { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="log"></param>
	/// <param name="userEncryption"></param>
	public GetUserEncryptionKeyHandler(ILog<GetUserEncryptionKeyHandler> log, IUserEncryptionRepository userEncryption) =>
		(Log, UserEncryption) = (log, userEncryption);

	/// <summary>
	/// Get encryption key for a user and decrypt it using the user's password
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<string>> HandleAsync(GetUserEncryptionKeyQuery query)
	{
		Log.Vrb("Getting encryption key for user {UserId}.", query.UserId.Value);

		return UserEncryption
			.StartFluentQuery()
			.Where(ue => ue.UserId, Jeebs.Data.Enums.Compare.Equal, query.UserId)
			.Maximum(1)
			.ExecuteAsync(ue => ue.Key)
			.BindAsync(k => k.Unlock(query.Password))
			.AuditAsync(none: r => Log.Err("Unable to load encryption key for user {UserId}: {Reason}.", query.UserId.Value, r))
			.MapAsync(
				x => x.Contents,
				e => new M.UnableToReadUserEncryptionKeyContentsExceptionMsg(e)
			);
	}

	public static class M
	{
		/// <summary>Unable to read </summary>
		/// <param name="Value">Exception object</param>
		public sealed record class UnableToReadUserEncryptionKeyContentsExceptionMsg(Exception Value) : ExceptionMsg;
	}
}
