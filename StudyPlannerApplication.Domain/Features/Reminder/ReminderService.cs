using StudyPlannerApplication.Domain.Features.Course;

namespace StudyPlannerApplication.Domain.Features.Reminder;

public class ReminderService
{
    private readonly DapperService _dapper;

    public ReminderService(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<ReminderResponseModel> GetAllCourse(ReminderRequestModel reqModel)
    {
        ReminderResponseModel model = new ReminderResponseModel();
        List<SubjectListModel> subList = new List<SubjectListModel>();
        try
        {
            string courseQuery = @"select s.SubjectName,c.* from 
                                   Tbl_Course c inner join
                                   Tbl_Subject s on c.SubjectCode=s.SubjectCode
                                   where CONVERT(DATE, c.DueDate) = CONVERT(DATE, GETDATE())
                                   and c.CreatedUserId=@CurrentUserId";
            var result = _dapper.Query<CourseDataModel>(courseQuery, reqModel).ToList();
            if (result.Count <= 0)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Data found.", true);
                return model;
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
            model.Response = SubResponseModel.GetResponseMsg("Course data are successfully retrieved.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }
}
