// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Security.Claims;
using MaybeF.Extensions;

namespace WebApp;

public static class UserExtensions
{
	public static Maybe<string> GetEncryptionKey(this ClaimsPrincipal principal) =>
		principal.Claims
			.SingleOrNone(
				c => c.Type == Domain.ClaimTypes.EncryptionKey
			)
			.Bind(
				c => c.Value.Some()
			);
}
