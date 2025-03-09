using StudyPlannerApplication.Database.EFAppDbContextModels;
using StudyPlannerApplication.Domain.Features.Course;
using System.Security;

namespace StudyPlannerApplication.Domain.Features.Dashboard;

public class DashboardService
{
    private readonly DapperService _dapper;
    private readonly AppDbContext _db;

    public DashboardService(DapperService dapper, AppDbContext db)
    {
        _dapper = dapper;
        _db = db;
    }

    public async Task<Result<DashboardResponseModel>> GetAllCourseList(DashboardRequestModel reqModel)
    {
        DashboardResponseModel model = new();
        List<CurrentWeekDataModel> currentWeekDayList = new();
        DateTime currentWeekStartDate = new();
        DateTime currentDay = DateTime.Now;
        DayOfWeek day = DateTime.Now.DayOfWeek;
        try
        {
            if (currentDay.DayOfWeek != DayOfWeek.Sunday)
            {
                currentWeekStartDate = currentDay.AddDays(-(int)day + 1);
            }
            else
            {
                currentWeekStartDate = currentDay.AddDays(-6);
            }
            DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);
            string courseQuery = @"select s.SubjectName,c.* from 
                                   Tbl_Course c inner join
                                   Tbl_Subject s on c.SubjectCode=s.SubjectCode
                                   where CONVERT(DATE, c.DueDate) between Convert(Date,@StartDate) and CONVERT(Date,@EndDate)
                                   and c.CreatedUserId=@CurrentUserId";
            reqModel.StartDate = currentWeekStartDate;
            reqModel.EndDate = currentWeekEndDate;
            var result = _dapper.Query<CourseDataModel>(courseQuery, reqModel).ToList();
            for (int i = 0; i < 7; i++)
            {
                CurrentWeekDataModel currentWeekDay = new();
                currentWeekDay.DateValue = currentWeekStartDate.AddDays(i);
                currentWeekDay.DayName = currentWeekDay.DateValue.DayOfWeek.ToString();
                currentWeekDay.CourseList = result.Where(x => x.DueDate.Date == currentWeekDay.DateValue.Date).ToList();
                currentWeekDayList.Add(currentWeekDay);
            }
            model.CurrentWeekDataList = currentWeekDayList;
            return Result<DashboardResponseModel>.SuccessResult(model,"Courses are retrieved successfully.");
        }
        catch(Exception ex)
        {
            return Result<DashboardResponseModel>.FailureResult(ex.ToString());
        }
    }
}
