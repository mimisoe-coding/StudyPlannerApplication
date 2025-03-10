namespace StudyPlannerApplication.Domain.Features.Reminder;

public class ReminderResponseModel 
{
    public List<SubjectListModel> SubjectList { get; set; }
}

public class SubjectListModel
{
    public string SubjectName { get; set; }
    public List<CourseDataModel> CourseList { get; set; }
}