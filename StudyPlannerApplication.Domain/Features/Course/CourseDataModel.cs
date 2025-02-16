namespace StudyPlannerApplication.Domain.Features.Course;

public class CourseDataModel
{
    public int CourseId { get; set; }   
    public string CourseName { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime DueDate { get; set; }
    public string SubjectCode { get; set; }
    public string SubjectNmae { get; set; } 

}
