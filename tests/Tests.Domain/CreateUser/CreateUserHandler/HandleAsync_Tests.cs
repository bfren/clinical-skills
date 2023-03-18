// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Repositories;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Auth.Data.Entities;

namespace ClinicalSkills.Domain.CreateUser.CreateUserHandler_Tests;

public sealed class HandleAsync_Tests : Abstracts.TestHandler
{
	private sealed class Setup : Setup<IAuthUserRepository, AuthUserEntity, AuthUserId, CreateUserHandler, Setup.CreateUserVars>
	{
		internal override CreateUserHandler GetHandler(CreateUserVars v) =>
			new(v.Repo, v.UserEncryption, v.Log);

		internal sealed record class CreateUserVars : Vars<IAuthUserRepository, AuthUserEntity, AuthUserId, CreateUserHandler>
		{
			public IUserEncryptionRepository UserEncryption { get; init; } = Substitute.For<IUserEncryptionRepository>();
		}
	}

	private (CreateUserHandler, Setup.CreateUserVars) GetVars() =>
		new Setup().GetVars();

	[Fact]
	public async Task Logs_To_Vrb__With_Query_Using_Redacted_Password()
	{
		// Arrange
		var (handler, v) = GetVars();
		var query = new CreateUserQuery(Rnd.Str, Rnd.Str, Rnd.Str);

		// Act
		await handler.HandleAsync(query);

		// Assert
		v.Log.Received().Vrb("Create User: {Query}", query with { Password = "** REDACTED **" });
	}

	[Fact]
	public async void Calls_Repo_CreateAsync__With_Correct_Values()
	{
		// Arrange
		var (handler, v) = GetVars();
		var name = Rnd.Str;
		var email = Rnd.Str;
		var password = Rnd.Str;
		var query = new CreateUserQuery(name, email, password);

		// Act
		await handler.HandleAsync(query);

		// Assert
		await v.Repo.Received().CreateAsync(email, password, name);
	}

	[Fact]
	public async void Calls_Repo_CreateAsync__Returns_Result()
	{
		// Arrange
		var (handler, v) = GetVars();
		var expectedUserId = LongId<AuthUserId>();
		var expectedUserEncryptionId = LongId<UserEncryptionId>();
		v.Repo.CreateAsync(email: default!, plainTextPassword: default!, friendlyName: default)
			.ReturnsForAnyArgs(expectedUserId);
		v.UserEncryption.CreateAsync(entity: default!)
			.ReturnsForAnyArgs(expectedUserEncryptionId);
		var query = new CreateUserQuery(Rnd.Str, Rnd.Str, Rnd.Str);

		// Act
		var result = await handler.HandleAsync(query);

		// Assert
		var some = result.AssertSome();
		Assert.Equal(expectedUserId, some);
	}
}
