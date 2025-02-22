using StudyPlannerApplication.Domain.Features.Course;

namespace StudyPlannerApplication.Domain.Features.Reminder;

public class ReminderResponseModel 
{
    public ResponseModel Response { get; set; }
    public List<SubjectListModel> SubjectList { get; set; }
}

public class SubjectListModel
{
    public string SubjectName { get; set; }
    public List<CourseDataModel> CourseList { get; set; }
}