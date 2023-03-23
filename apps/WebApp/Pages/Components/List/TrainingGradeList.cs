// Clinical Skills Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Domain.Queries.GetTrainingGrades;
using Persistence.StrongIds;

namespace WebApp.Pages.Components.List;

public sealed class TrainingGradeListViewComponent : ListSingleViewComponent<TrainingGradesModel, TrainingGradeId>
{
	public TrainingGradeListViewComponent() : base("training grade", x => x.Name, x => x.Name) { }
}
