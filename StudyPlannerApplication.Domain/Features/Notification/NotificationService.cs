namespace StudyPlannerApplication.Domain.Features.Notification;

public class NotificationService
{
    private readonly DapperService _dapper;

    public NotificationService(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<NotificationResponseModel> GetAllNotification(NotificationRequestModel reqModel)
    {
        NotificationResponseModel model = new();
        try
        {
            string query = @"SELECT n.*,e.SubjectCode,s.SubjectName,DATEDIFF(DAY,GETDATE(),e.DueDate) as Day
FROM Tbl_Notification n
INNER JOIN Tbl_Exam e ON e.ExamId = n.ExamId
Inner Join Tbl_Subject s ON s.SubjectCode = e.SubjectCode
WHERE e.DueDate BETWEEN GETDATE() AND DATEADD(DAY, 30, GETDATE())
and e.CreatedUserId=@CurrentUserId";
            var result = _dapper.Query<NotificationDataModel>(query,reqModel);
            foreach(var item in result)
            {
                item.Message = string.Format(item.Message, item.Day, item.SubjectName);
            }
            model.NotiList = result;
            model.Response = SubResponseModel.GetResponseMsg("Notification success", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }
}
