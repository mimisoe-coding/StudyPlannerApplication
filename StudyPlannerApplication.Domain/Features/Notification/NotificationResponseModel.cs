namespace StudyPlannerApplication.Domain.Features.Notification;

public class NotificationResponseModel
{
    public List<NotificationDataModel> NotiList { get; set; }
    public PageSettingResponseModel PageSetting { get; set; }
}

public class NotificationDataModel
{
    public int NotificationId { get; set; }
    public int ExamId { get; set; }
    public string Message { get; set; }
    public string UserId { get; set; }
    public string SubjectCode { get; set; }
    public string SubjectName { get; set; }
    public int Day { get; set; }
}