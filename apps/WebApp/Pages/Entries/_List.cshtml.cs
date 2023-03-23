// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.GetEntries;

namespace WebApp.Pages.Entries;

public sealed record class ListModel
{
	public IEnumerable<EntryModel> Entries { get; init; }

	public string DeleteHandler { get; init; }

	public string ReplaceId { get; init; }

	public ListModel(string deleteHandler) : this(deleteHandler, string.Empty) { }

	public ListModel(string deleteHandler, string replaceId) =>
		(Entries, DeleteHandler, ReplaceId) = (new List<EntryModel>(), deleteHandler, replaceId);
}
