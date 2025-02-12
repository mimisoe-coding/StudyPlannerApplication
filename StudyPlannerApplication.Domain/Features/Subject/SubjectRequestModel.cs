namespace StudyPlannerApplication.Domain.Features.Subject;

public class SubjectRequestModel : BaseRequestModel
{
    public string SubjectName { get; set; }
    public string Description { get; set; }
}
