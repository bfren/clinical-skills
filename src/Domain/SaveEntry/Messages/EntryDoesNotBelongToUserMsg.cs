// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Auth.Data;
using Jeebs.Messages;

namespace ClinicalSkills.Domain.SaveEntry.Messages;

/// <summary>Entry does not belong to a user</summary>
/// <param name="UserId"></param>
/// <param name="EntryId"></param>
public sealed record class EntryDoesNotBelongToUserMsg(
	AuthUserId UserId,
	EntryId EntryId
) : Msg;
