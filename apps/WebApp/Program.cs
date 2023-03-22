// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs;
using Jeebs.Cqrs;
using WebApp;
using D = Domain;

// ==========================================
//  CONFIGURE
// ==========================================

var (app, log) = Jeebs.Apps.Web.MvcApp.Create<App>(args);
var dispatcher = app.Services.GetRequiredService<IDispatcher>();

// ==========================================
//  MIGRATE
// ==========================================

log.Inf("Migrate database to latest version.");
_ = await dispatcher
	.DispatchAsync(
		new D.MigrateToLatestCommand()
	)
	.LogBoolAsync(
		log
	);

// ==========================================
//  INSERT TEST DATA
// ==========================================

// Get environment variable shorthand
static string? Env(string key) =>
	Environment.GetEnvironmentVariable(key);

if (Env("TRUNCATE") == "true")
{
	log.Wrn("Truncating database tables.");
	_ = await dispatcher
		.DispatchAsync(
			new D.TruncateEverythingCommand()
		)
		.LogBoolAsync(
			log
		);

	log.Inf("Inserting test data.");
	_ = await dispatcher
		.DispatchAsync(
			new D.InsertTestDataCommand()
		)
		.LogBoolAsync(
			log
		);
}
else if (Env("CLINICALSKILLS_USER_EMAIL") is string email && Env("CLINICALSKILLS_USER_PASS") is string pass)
{
	var name = Env("CLINICALSKILLS_USER_NAME") ?? "Default";
	log.Inf("Attempting to create user {Name} with {Email}.", name, email);
	_ = await dispatcher
		.DispatchAsync(
			new D.CreateUserQuery(name, email, pass)
		)
		.AuditAsync(
			some: x => log.Inf("Created user {Id}.", x.Value),
			none: log.Msg
		);
}

// ==========================================
//  RUN APP
// ==========================================

app.Run();
