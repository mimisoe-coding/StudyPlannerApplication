namespace StudyPlannerApplication.Domain.Features.Reminder;

public class ReminderService
{
    private readonly DapperService _dapper;

    public ReminderService(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<Result<ReminderResponseModel>> GetAllCourse(ReminderRequestModel reqModel)
    {
        ReminderResponseModel model = new ReminderResponseModel();
        List<SubjectListModel> subList = new List<SubjectListModel>();
        try
        {
            string courseQuery = @"select s.SubjectName,c.* from 
                                   Tbl_Course c inner join
                                   Tbl_Subject s on c.SubjectCode=s.SubjectCode
                                   where CONVERT(DATE, c.DueDate) = CONVERT(DATE, GETDATE())
                                    and Status='Pending'
                                   and c.CreatedUserId=@CurrentUserId";
            var result = _dapper.Query<CourseDataModel>(courseQuery, reqModel).ToList();
            if (result.Count <= 0)
            {
                return Result<ReminderResponseModel>.SuccessResult("No Data found.");
            }

            var subjectNameList = result.Select(x => x.SubjectName).Distinct();
            foreach (var name in subjectNameList)
            {
                SubjectListModel subject = new SubjectListModel();
                var courseList = result.Where(x => x.SubjectName == name).ToList();
                subject.SubjectName = name;
                subject.CourseList = courseList;
                subList.Add(subject);
            }
            model.SubjectList = subList;
            return Result<ReminderResponseModel>.SuccessResult(model,"Course data are successfully retrieved.");
        }
        catch (Exception ex)
        {
            return Result<ReminderResponseModel>.FailureResult(ex.ToString());
        }
    }
}
