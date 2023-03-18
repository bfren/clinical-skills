// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Domain.GetUserEncryptionKey;
using ClinicalSkills.Persistence.Entities;
using ClinicalSkills.Persistence.Repositories;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Cryptography;
using Jeebs.Cryptography.Functions;
using Jeebs.Data.Enums;
using Jeebs.Data.Testing.Query;

namespace ClinicalSkills.Domain.GetUserProfile.GetUserEncryptionKeyHandler_Tests;

public sealed class HandleAsync_Tests
{
	internal sealed class TestHandler : Abstracts.TestHandlerBase<IUserEncryptionRepository, UserEncryptionEntity, UserEncryptionId, GetUserEncryptionKeyHandler>
	{
		internal sealed class Setup : SetupBase
		{
			internal override GetUserEncryptionKeyHandler GetHandler(Vars v) =>
				new(v.Log, v.Repo);
		}
	}

	internal (GetUserEncryptionKeyHandler, TestHandler.Vars) GetVars() =>
		new TestHandler.Setup().GetVars();

	[Fact]
	public async Task Logs_To_Vrb__With_Query()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var query = new GetUserEncryptionKeyQuery(userId, Rnd.Str);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Getting encryption key for user {UserId}.", userId.Value);
	}

	[Fact]
	public async Task Calls_FluentQuery_Where__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var query = new GetUserEncryptionKeyQuery(userId, Rnd.Str);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			c => FluentQueryHelper.AssertWhere<UserEncryptionEntity, AuthUserId>(c, x => x.UserId, Compare.Equal, userId),
			_ => { },
			_ => { }
		);
	}

	[Fact]
	public async Task Calls_FluentQuery_ExecuteAsync__With_Correct_Column()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new GetUserEncryptionKeyQuery(LongId<AuthUserId>(), Rnd.Str);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Fluent.AssertCalls(
			_ => { },
			_ => { },
			c => FluentQueryHelper.AssertExecute<UserEncryptionEntity, Locked<string>>(c, x => x.Key, false)
		);
	}

	[Fact]
	public async Task User_Encryption_Key_Does_Not_Exist__Logs_Error__Returns_None()
	{
		// Arrange
		var (handler, v) = GetVars();
		var userId = LongId<AuthUserId>();
		var query = new GetUserEncryptionKeyQuery(userId, Rnd.Str);

		v.Fluent.ExecuteAsync<Locked<string>>(aliasSelector: default!)
			.ReturnsForAnyArgs(Create.None<Locked<string>>());

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Err("Unable to load encryption key for user {UserId}: {Reason}.", userId.Value, Arg.Any<IMsg>());
		result.AssertNone();
	}

	[Fact]
	public async Task User_Encryption_Key_Exists__Returns_Encryption_Key()
	{
		// Arrange
		var (handler, v) = GetVars();
		var password = Rnd.Str;
		var expected = Rnd.Str;
		var query = new GetUserEncryptionKeyQuery(LongId<AuthUserId>(), password);

		v.Fluent.ExecuteAsync<Locked<string>>(aliasSelector: default!)
			.ReturnsForAnyArgs(CryptoF.Lock(expected, password));

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		var actual = result.AssertSome();
		Assert.Equal(expected, actual);
	}
}
