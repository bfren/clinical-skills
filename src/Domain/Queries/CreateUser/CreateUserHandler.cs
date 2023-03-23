// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Cryptography;
using Jeebs.Logging;
using Persistence.Repositories;
using RndF;

namespace Domain.Queries.CreateUser;

/// <summary>
/// Create a new user
/// </summary>
internal sealed class CreateUserHandler : QueryHandler<CreateUserQuery, AuthUserId>
{
	private ILog<CreateUserHandler> Log { get; init; }

	private IAuthUserRepository User { get; init; }

	private IUserEncryptionRepository UserEncryption { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="user"></param>
	/// <param name="userEncryption"></param>
	/// <param name="log"></param>
	public CreateUserHandler(IAuthUserRepository user, IUserEncryptionRepository userEncryption, ILog<CreateUserHandler> log) =>
		(User, UserEncryption, Log) = (user, userEncryption, log);

	/// <summary>
	/// Create a new user from <paramref name="query"/>
	/// </summary>
	/// <param name="query"></param>
	public override Task<Maybe<AuthUserId>> HandleAsync(CreateUserQuery query)
	{
		Log.Vrb("Create User: {Query}", query with { Password = "** REDACTED **" });

		var key = Rnd.StringF.Get(64).Lock(query.Password);
		return from u in User.CreateAsync(query.EmailAddress, query.Password, query.Name)
			   from e in UserEncryption.CreateAsync(new() { UserId = u, Key = key })
			   select u;
	}
}
