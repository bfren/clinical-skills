// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence;
using ClinicalSkills.Persistence.Clients.PostgreSql;
using ClinicalSkills.Persistence.StrongIds;
using Jeebs.Apps.Web;
using Jeebs.Auth.Data.Clients.PostgreSql;
using Jeebs.Cqrs;
using Jeebs.Mvc.Auth;
using MaybeF.Caching;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using StrongId.Mvc;

namespace ClinicalSkills.WebApp;

public sealed class App : RazorApp
{
	public override void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
	{
		base.ConfigureServices(ctx, services);

		_ = services.AddClinicalSkillsData();
		_ = services.AddClinicalSkillsMigrator();

		_ = services.AddAuthentication(ctx.Configuration)
			.WithData<PostgreSqlDbClient>(true);

		_ = services
			.AddCqrs();

		_ = services
			.AddMemoryCache()
			.AddMaybeCache<ClinicalSettingId>();
	}

	protected override void ConfigureResponseCompression(WebApplication app)
	{
		if (app.Environment.IsProduction())
		{
			base.ConfigureResponseCompression(app);
		}
	}

	protected override void ConfigureServicesMvcOptions(HostBuilderContext ctx, MvcOptions opt)
	{
		base.ConfigureServicesMvcOptions(ctx, opt);
		opt.AddStrongIdModelBinder();
	}

	protected override void ConfigureAuth(WebApplication app, IConfiguration config)
	{
		_ = app.UseAuthentication();
		base.ConfigureAuth(app, config);
	}

	public override void ConfigureSerilog(HostBuilderContext ctx, LoggerConfiguration loggerConfig)
	{
		base.ConfigureSerilog(ctx, loggerConfig);
		_ = loggerConfig.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning);
	}
}
