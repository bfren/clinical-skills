// Clinical Skills
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Auth.Data;
using Jeebs.Messages;
using Persistence.StrongIds;

namespace Domain.Queries.SaveSkill.Messages;

/// <summary>Skill does not belong to a user</summary>
/// <param name="UserId"></param>
/// <param name="SkillId"></param>
public sealed record class SkillDoesNotBelongToUserMsg(
	AuthUserId UserId,
	SkillId SkillId
) : Msg;
