// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain;
using Domain.Commands.SaveUserSettings.Messages;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Persistence.Entities;
using Persistence.Repositories;
using Persistence.StrongIds;
using StrongId;

namespace Abstracts.UpdateSettings;

public abstract class HandleAsync_Tests
{
	public abstract Task Test00_ItemId_Is_Not_Null__Checks_Item_Belongs_To_User__Receives_None__Returns_None_With_SaveUserSettingsCheckFailedMsg();

	public abstract Task Test01_ItemId_Is_Not_Null__Checks_Item_Belongs_To_User__Receives_Some_False__Returns_None_With_SaveUserSettingsCheckFailedMsg();

	public abstract Task Test02_ItemId_Is_Not_Null__Checks_Item_Belongs_To_User__Receives_Some_True__Calls_Log_Vrb__With_Correct_Values();

	public abstract Task Test03_ItemId_Is_Not_Null__Checks_Item_Belongs_To_User__Receives_Some_True__Calls_UserSettings_UpdateAsync__With_Correct_Values();

	public abstract Task Test04_ItemId_Is_Null__Calls_Log_Vrb__With_Correct_Values();

	public abstract Task Test05_ItemId_Is_Null__Calls_UserSettings_UpdateAsync__With_Correct_Values();

	internal abstract class TestHandlerBase<TCommand, THandler, TItemId> : TestHandlerBase<IUserSettingsRepository, UserSettingsEntity, UserSettingsId, THandler>
		where TCommand : Command, IWithId<UserSettingsId>, IWithUserId
		where THandler : CommandHandler<TCommand>
		where TItemId : LongId, new()
	{
		internal new abstract class SetupBase : SetupBase<Vars>
		{
			protected SetupBase(string name) : base(name) { }
		}

		internal new abstract class SetupBase<TVars> : TestHandlerBase<IUserSettingsRepository, UserSettingsEntity, UserSettingsId, THandler>.SetupBase<TVars>
			where TVars : Vars, new()
		{
			internal string Name { get; }

			internal abstract TCommand GetCommand(AuthUserId? userId = null, TItemId? itemId = null);

			protected SetupBase(string name) =>
				Name = name;

			internal async Task Test00<TSaveUserSettingsCheckFailedMsg>(Func<THandler, TCommand, Task<Maybe<bool>>> handle)
				where TSaveUserSettingsCheckFailedMsg : IMsg
			{
				// Arrange
				var (handler, v) = GetVars();
				var command = GetCommand(itemId: LongId<TItemId>());
				v.Dispatcher.SendAsync<bool>(query: default!)
					.ReturnsForAnyArgs(Create.None<bool>());

				// Act
				var result = await handle(handler, command);

				// Assert
				result.AssertNone().AssertType<TSaveUserSettingsCheckFailedMsg>();
			}

			internal async Task Test01<TSaveUserSettingsCheckFailedMsg>(Func<THandler, TCommand, Task<Maybe<bool>>> handle)
				where TSaveUserSettingsCheckFailedMsg : IMsg
			{
				// Arrange
				var (handler, v) = GetVars();
				var command = GetCommand(itemId: LongId<TItemId>());
				v.Dispatcher.SendAsync<bool>(query: default!)
					.ReturnsForAnyArgs(F.False);

				// Act
				var result = await handle(handler, command);

				// Assert
				result.AssertNone().AssertType<TSaveUserSettingsCheckFailedMsg>();
			}

			internal async Task Test02(Func<THandler, TCommand, Task<Maybe<bool>>> handle)
			{
				// Arrange
				var (handler, v) = GetVars();
				var userId = LongId<AuthUserId>();
				var command = GetCommand(userId, LongId<TItemId>());
				v.Dispatcher.SendAsync<bool>(query: default!)
					.ReturnsForAnyArgs(F.True);

				// Act
				_ = await handle(handler, command);

				// Assert
				v.Log.Received().Vrb($"Updating Default {Name} for {{User}}.", userId.Value);
			}

			internal async Task Test03(Func<THandler, TCommand, Task<Maybe<bool>>> handle)
			{
				// Arrange
				var (handler, v) = GetVars();
				var command = GetCommand(itemId: LongId<TItemId>());
				v.Dispatcher.SendAsync<bool>(query: default!)
					.ReturnsForAnyArgs(F.True);

				// Act
				_ = await handle(handler, command);

				// Assert
				await v.Repo.Received().UpdateAsync(command);
			}

			internal async Task Test04(Func<THandler, TCommand, Task<Maybe<bool>>> handle)
			{
				// Arrange
				var (handler, v) = GetVars();
				var userId = LongId<AuthUserId>();
				var command = GetCommand(userId);

				// Act
				_ = await handle(handler, command);

				// Assert
				v.Log.Received().Vrb($"Updating Default {Name} for {{User}}.", userId.Value);
			}

			internal async Task Test05(Func<THandler, TCommand, Task<Maybe<bool>>> handle)
			{
				// Arrange
				var (handler, v) = GetVars();
				var command = GetCommand();

				// Act
				_ = await handle(handler, command);

				// Assert
				await v.Repo.Received().UpdateAsync(command);
			}
		}
	}
}
