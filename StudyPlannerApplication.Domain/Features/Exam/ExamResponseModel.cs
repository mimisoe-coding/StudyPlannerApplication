namespace StudyPlannerApplication.Domain.Features.Exam;

public class ExamResponseModel 
{
    public List<ExamDataModel> ExamList { get; set; }
    public ExamDataModel ExamData { get; set; }
    public ResponseModel Response { get; set; }
    public PageSettingResponseModel PageSetting { get; set; }
}

public class ExamDataModel
{
    public int ExamId { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime DueDate { get; set; }
    public string SubjectCode { get; set; }
    public DateTime Duration { get; set; }
    public TimeSpan Durationtime { get; set; }
    public string SubjectName { get; set; }
}
