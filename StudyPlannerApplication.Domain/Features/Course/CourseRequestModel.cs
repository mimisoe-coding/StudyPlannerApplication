namespace StudyPlannerApplication.Domain.Features.Course;

public class CourseRequestModel: BaseRequestModel
{
    public int CourseId { get; set; }   
    public string CourseName { get; set; } 
    public string Description { get; set; }
    public string Status { get; set; }  
    public string SubjectCode { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Now;
    public PageSettingModel PageSetting { get; set; }
}
