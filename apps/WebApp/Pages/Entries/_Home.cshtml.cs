// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Domain;
using Jeebs.Auth.Data;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Jeebs.Mvc.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalSkills.WebApp.Pages.Entries;

public sealed record class HomeModel
{
	public RecentModel RecentEntries { get; set; } = new();
}

public sealed partial class IndexModel
{
	public Task<PartialViewResult> OnGetHomeAsync() =>
		CreateHomeModel(
			User.GetUserId(), Dispatcher, Log
		)
		.SwitchAsync(
			some: x => Partial("_Home", x),
			none: r => Partial("_Error", r)
		);

	internal static Task<Maybe<HomeModel>> CreateHomeModel(Maybe<AuthUserId> user, IDispatcher dispatcher, ILog log)
	{
		var query = from u in user
					from recent in dispatcher.DispatchAsync(new GetRecentEntriesQuery(u))
					select new { recent };

		return query
			.AuditAsync(none: log.Msg)
			.MapAsync(
				x => new HomeModel
				{
					RecentEntries = new() { Entries = x.recent.ToList() }
				},
				F.DefaultHandler
			);
	}
}
