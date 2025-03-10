namespace StudyPlannerApplication.Domain.Features.Subject;

public class SubjectResponseModel
{
    public List<SubjectDataModel> SubjectList { get; set; }
    public SubjectDataModel Subject { get; set; }
    public PageSettingResponseModel PageSetting { get; set; }
}
