// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Domain;
using ClinicalSkills.Domain.GetRecentEntries;
using Jeebs.Mvc.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalSkills.WebApp.Pages.Entries;

public sealed record class RecentModel
{
	public List<RecentEntryModel> Entries { get; init; } = new();
}

public sealed partial class IndexModel
{
	public Task<PartialViewResult> OnGetRecentAsync()
	{
		var query = from u in User.GetUserId()
					from j in Dispatcher.DispatchAsync(new GetRecentEntriesQuery(u))
					select j;

		return query
			.AuditAsync(none: Log.Msg)
			.SwitchAsync(
				some: x => Partial("_Recent", new RecentModel { Entries = x.ToList() }),
				none: _ => Partial("_Recent", new RecentModel())
			);
	}
}
