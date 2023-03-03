// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ClinicalSkills.WebApp;
using Jeebs.Cqrs;

// ==========================================
//  CONFIGURE
// ==========================================

var (app, log) = Jeebs.Apps.Web.MvcApp.Create<App>(args);
var dispatcher = app.Services.GetRequiredService<IDispatcher>();

// ==========================================
//  RUN APP
// ==========================================

app.Run();
