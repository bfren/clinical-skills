// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Security.Claims;
using ClinicalSkills.Domain.GetUserEncryptionKey;
using Jeebs.Auth;
using Jeebs.Auth.Data;
using Jeebs.Auth.Data.Models;
using Jeebs.Config.Web.Auth;
using Jeebs.Cqrs;
using Jeebs.Logging;
using Jeebs.Mvc.Auth.Functions;
using Microsoft.Extensions.Options;

namespace ClinicalSkills.WebApp.Pages.Auth;

public sealed class SignInModel : Jeebs.Mvc.Razor.Pages.Auth.SignInModel
{
	private readonly IDispatcher dispatcher;

	public SignInModel(
		IAuthDataProvider auth,
		IAuthJwtProvider jwt,
		IDispatcher dispatcher,
		IOptions<AuthConfig> config,
		ILog<SignInModel> log) : base(auth, jwt, config, log
	) =>
		this.dispatcher = dispatcher;

	protected override AuthF.GetClaims? GetClaims =>
		GetEncryptionKeyAsync;

	private Task<List<Claim>> GetEncryptionKeyAsync(AuthUserModel user, string password) =>
		dispatcher
			.DispatchAsync(
				new GetUserEncryptionKeyQuery(user.Id, password)
			)
			.SwitchAsync(
				some: k => new List<Claim> { new(Domain.ClaimTypes.EncryptionKey, k) },
				none: r => new List<Claim>()
			);
}
