// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.WebApp;
using Jeebs;
using Jeebs.Cqrs;
using D = ClinicalSkills.Domain;

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
		new D.MigrateToLatest.MigrateToLatestCommand()
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
		.DispatchAsync(new D.TruncateEverything.TruncateEverythingCommand())
		.LogBoolAsync(log);

	log.Inf("Inserting test data.");
	_ = await dispatcher
		.DispatchAsync(new D.InsertTestData.InsertTestDataCommand())
		.LogBoolAsync(log);
}
else if (Env("CLINICALSKILLS_USER_EMAIL") is string email && Env("CLINICALSKILLS_USER_PASS") is string pass)
{
	var name = Env("CLINICALSKILLS_USER_NAME") ?? "Default";
	log.Inf("Attempting to create user {Name} with {Email}.", name, email);
	_ = await dispatcher
		.DispatchAsync(
			new D.CreateUser.CreateUserQuery(name, email, pass)
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
