// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Commands;
using Domain.Queries.GetTrainingGrades;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.Settings.General;

public sealed class EditTrainingGradeModel : EditSettingsModel
{
	public List<TrainingGradesModel> TrainingGrades { get; set; } = new();

	public EditTrainingGradeModel() : base("Default Training Grade") { }
}

public sealed partial class IndexModel
{
	public Task<PartialViewResult> OnGetEditTrainingGradeAsync() =>
		GetFieldAsync("TrainingGrade",
			x => Dispatcher.SendAsync(new Q.GetTrainingGradesQuery(x, false)),
			(s, v) => new EditTrainingGradeModel { Settings = s, TrainingGrades = v.ToList() }
		);

	public Task<IActionResult> OnPostEditTrainingGradeAsync(UpdateDefaultTrainingGradeCommand settings) =>
		PostFieldAsync("TrainingGrade", "Default Training Grade", settings, x => x.DefaultTrainingGradeId);
}
