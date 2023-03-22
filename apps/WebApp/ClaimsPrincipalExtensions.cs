// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Security.Claims;
using Jeebs.Cryptography;
using Jeebs.Logging;
using MaybeF.Extensions;

namespace WebApp;

public static class ClaimsPrincipalExtensions
{
	public static string Decrypt<T>(this ClaimsPrincipal @this, Locked<T> box)
	{
		var contents = from key in GetEncryptionKey(@this)
					   from unlocked in box.Unlock(key)
					   select unlocked.Contents;

		return contents
			.Audit(
				none: StaticLogger.Log.Msg
			)
			.Switch(
				some: x => x.ToString() ?? "*empty*",
				none: "*encrypted*"
			);
	}

	public static Maybe<string> GetEncryptionKey(this ClaimsPrincipal @this) =>
		@this.Claims
			.SingleOrNone(
				c => c.Type == Domain.ClaimTypes.EncryptionKey
			)
			.Bind(
				c => c.Value.Some()
			);
}
