// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.Persistence.Clients.Sqlite;
using Jeebs.Cqrs;
using Microsoft.Extensions.DependencyInjection;
using D = ClinicalSkills.Domain;

// ==========================================
//  CONFIGURE
// ==========================================

var (app, log) = Jeebs.Apps.Host.Create(args, (_, svc) => svc.AddData().AddCqrs());

// ==========================================
//  BEGIN
// ==========================================

log.Inf("Clinical Skills Console app.");

// ==========================================
//  SETUP
// ==========================================

var dispatcher = app.Services.GetRequiredService<IDispatcher>();

static void Write(string text)
{
	var pad = new string('=', text.Length + 6);
	Console.WriteLine();
	Console.WriteLine(pad);
	Console.WriteLine($"== {text} ==");
	Console.WriteLine(pad);
}

static void Pause()
{
	Write("PAUSE");
	Console.WriteLine("Press any key when ready.");
	_ = Console.ReadLine();
}

// ==========================================
//  RUN MIGRATIONS
// ==========================================

Write("Migrations");
log.Inf("Migrate to latest database version.");
await dispatcher.DispatchAsync(
	new D.MigrateToLatest.MigrateToLatestCommand()
);

// ==========================================
//  DONE
// ==========================================

Write("Done");
