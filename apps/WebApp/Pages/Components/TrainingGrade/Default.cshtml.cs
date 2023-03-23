// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;
using Jeebs.Logging;
using Jeebs.Mvc.Auth;
using Microsoft.AspNetCore.Mvc;
using Persistence.StrongIds;

namespace WebApp.Pages.Components.TrainingGrade;

public sealed record class TrainingGradeModel(string Label, string? UpdateUrl, TrainingGradeId Id, string Name, EntryId? EntryId)
{
	public static TrainingGradeModel Blank(string label, string? updateUrl) =>
		new(label, updateUrl, new(), string.Empty, null);
}

public sealed class TrainingGradeViewComponent : ViewComponent
{
	private IDispatcher Dispatcher { get; }

	private ILog Log { get; }

	public TrainingGradeViewComponent(IDispatcher dispatcher, ILog<TrainingGradeViewComponent> log) =>
		(Dispatcher, Log) = (dispatcher, log);

	public async Task<IViewComponentResult> InvokeAsync(string label, string updateUrl, TrainingGradeId value, EntryId? entryId)
	{
		if (value is null)
		{
			return View(TrainingGradeModel.Blank(label, updateUrl));
		}

		Log.Dbg("Get training grade: {TrainingGradeId}.", value);
		return await UserClaimsPrincipal
			.GetUserId()
			.BindAsync(x => Dispatcher.SendAsync(new Q.GetTrainingGradeQuery(x, value)))
			.AuditAsync(none: r => Log.Err("Unable to get training grade: {Reason}", r))
			.SwitchAsync(
				some: x => View(new TrainingGradeModel(label, updateUrl, x.Id, x.Name, entryId)),
				none: _ => (IViewComponentResult)View(TrainingGradeModel.Blank(label, updateUrl))
			);
	}
}
