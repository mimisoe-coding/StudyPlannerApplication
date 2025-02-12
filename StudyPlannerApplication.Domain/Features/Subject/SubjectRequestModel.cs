namespace StudyPlannerApplication.Domain.Features.Subject;

public class SubjectRequestModel : BaseRequestModel
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    public string Description { get; set; }
    public PageSettingModel PageSetting { get; set; }
}
