// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;
using Jeebs.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Entries;

[Authorize]
[ValidateAntiForgeryToken]
public sealed partial class IndexModel : PageModel
{
	private IDispatcher Dispatcher { get; init; }

	private ILog Log { get; init; }

	public IndexModel(IDispatcher dispatcher, ILog<IndexModel> log) =>
		(Dispatcher, Log) = (dispatcher, log);
}
