using StudyPlannerApplication.Domain.Features.Course;

namespace StudyPlannerApplication.Domain.Features.Dashboard;

public class DashboardResponseModel
{
    public List<CurrentWeekDataModel> CurrentWeekDataList { get; set; }
    public ResponseModel Response { get; set; }
}

public class CurrentWeekDataModel
{
    public string DayName { get; set; }
    public DateTime DateValue { get; set; }
    public List<CourseDataModel> CourseList { get; set; }
}
