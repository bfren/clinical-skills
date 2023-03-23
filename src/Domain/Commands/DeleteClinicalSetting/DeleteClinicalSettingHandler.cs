// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Data.Enums;
using Jeebs.Logging;
using MaybeF.Caching;
using Persistence;
using Persistence.Repositories;
using Persistence.StrongIds;

namespace Domain.Commands.DeleteClinicalSetting;

/// <summary>
/// Delete a clinical setting that belongs to a user
/// </summary>
internal sealed class DeleteClinicalSettingHandler : CommandHandler<DeleteClinicalSettingCommand>
{
	private IMaybeCache<ClinicalSettingId> Cache { get; init; }

	private IClinicalSettingRepository ClinicalSetting { get; init; }

	private IDispatcher Dispatcher { get; init; }

	private ILog<DeleteClinicalSettingHandler> Log { get; init; }

	/// <summary>
	/// Inject dependencies
	/// </summary>
	/// <param name="cache"></param>
	/// <param name="clinicalSetting"></param>
	/// <param name="dispatcher"></param>
	/// <param name="log"></param>
	public DeleteClinicalSettingHandler(IMaybeCache<ClinicalSettingId> cache, IClinicalSettingRepository clinicalSetting, IDispatcher dispatcher, ILog<DeleteClinicalSettingHandler> log) =>
		(Cache, ClinicalSetting, Dispatcher, Log) = (cache, clinicalSetting, dispatcher, log);

	/// <summary>
	/// Delete or disable the clinical setting specified in <paramref name="command"/>
	/// </summary>
	/// <param name="command"></param>
	public override Task<Maybe<bool>> HandleAsync(DeleteClinicalSettingCommand command) =>
		HandleAsync(command, DeleteOrDisableAsync);

	internal Task<Maybe<bool>> HandleAsync(DeleteClinicalSettingCommand command, DeleteOrDisable<ClinicalSettingId> dOrD)
	{
		Log.Vrb("Delete or Disable Clinical Setting: {Command}", command);
		return Dispatcher
			.SendAsync(new Queries.CheckClinicalSettingCanBeDeletedQuery(command.UserId, command.Id))
			.BindAsync(x => dOrD(command.UserId, command.Id, x))
			.IfSomeAsync(x => { if (x) { Cache.RemoveValue(command.Id); } });
	}

	/// <summary>
	/// Peform a delete or disable operation on a clinical setting
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="clinicalSettingId"></param>
	/// <param name="operation"></param>
	internal Task<Maybe<bool>> DeleteOrDisableAsync(AuthUserId userId, ClinicalSettingId clinicalSettingId, DeleteOperation operation) =>
		ClinicalSetting.StartFluentQuery()
			.Where(x => x.Id, Compare.Equal, clinicalSettingId)
			.Where(x => x.UserId, Compare.Equal, userId)
			.QuerySingleAsync<ClinicalSettingToDeleteModel>()
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: x => operation switch
				{
					DeleteOperation.Delete =>
						ClinicalSetting.DeleteAsync(x),

					DeleteOperation.Disable =>
						ClinicalSetting.UpdateAsync(x with { IsDisabled = true }),

					_ =>
						F.None<bool, Messages.ClinicalSettingCannotBeDeletedMsg>().AsTask()
				},
				none: _ => F.None<bool>(new Messages.ClinicalSettingDoesNotExistMsg(userId, clinicalSettingId))
			);
}
