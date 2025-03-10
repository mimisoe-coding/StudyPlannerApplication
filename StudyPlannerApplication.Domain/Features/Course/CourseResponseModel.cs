namespace StudyPlannerApplication.Domain.Features.Course;

public class CourseResponseModel
{
    public List<CourseDataModel> CourseList { get; set; }
    public CourseDataModel Course { get; set; }
    public PageSettingResponseModel PageSetting { get; set; }
}
