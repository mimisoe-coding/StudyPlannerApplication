namespace StudyPlannerApplication.Domain.Features.Exam;

public class ExamRequestModel:BaseRequestModel
{
    public int ExamId { get; set; }
    public string SubjectCode { get; set; }
    public string Description { get; set; }
    public TimeOnly Duration { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Now;
    public string Status { get; set; }
}
