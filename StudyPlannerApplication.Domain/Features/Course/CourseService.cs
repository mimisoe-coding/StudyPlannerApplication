using StudyPlannerApplication.Database.EFAppDbContextModels;
using StudyPlannerApplication.Domain.Features.Subject;

namespace StudyPlannerApplication.Domain.Features.Course;

public class CourseService
{
    private readonly AppDbContext _db;

    public CourseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<CourseResponseModel> Create(CourseRequestModel reqModel)
    {
        CourseResponseModel model = new CourseResponseModel();
        try
        {
            TblCourse course = new TblCourse()
            {
                CourseName = reqModel.CourseName,
                SubjectCode = reqModel.SubjectCode,
                Status = reqModel.Status,
                CreatedUserId = reqModel.CurrentUserId,
                Description = reqModel.Description,
                CreatedDate = DateTime.Now,
                DueDate = reqModel.DueDate,
            };

            await _db.AddAsync(course);
            await _db.SaveAndDetachAsync();
            model.Response = SubResponseModel.GetResponseMsg("Your course is successfully added.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<CourseResponseModel> List(CourseRequestModel reqModel)
    {
        CourseResponseModel model = new CourseResponseModel();
        PageSettingResponseModel pageSetting = new();
        try
        {
            var query = from c in _db.TblCourses
                        join sub in _db.TblSubjects
                        on c.SubjectCode equals sub.SubjectCode
                        where c.CreatedUserId == reqModel.CurrentUserId
                        select new CourseDataModel
                        {
                            CourseId = c.CourseId,
                            CourseName = c.CourseName,
                            SubjectCode = c.SubjectCode,
                            SubjectName = sub.SubjectName,
                            Description = c.Description,
                            Status = c.Status,
                            DueDate = c.DueDate
                        };
            var lst = query.ToList();

            pageSetting.TotalRowCount = lst.Count;
            model.PageSetting = pageSetting;
            model.CourseList = lst.Skip(reqModel.PageSetting.SkipRowCount)
                .Take(reqModel.PageSetting.PageSize).ToList();
            model.Response = SubResponseModel.GetResponseMsg("Your course is successfully added.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<CourseResponseModel> Delete(CourseRequestModel reqModel)
    {
        CourseResponseModel model = new CourseResponseModel();
        try
        {
            var item = await _db.TblCourses.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CourseId == reqModel.CourseId
                && x.CreatedUserId == reqModel.CurrentUserId);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Course Found", false);
                return model;
            }
            _db.Remove(item);
            await _db.SaveChangesAsync();
            model.Response = SubResponseModel.GetResponseMsg("Your course is successfully deleted", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<CourseResponseModel> Edit(int id)
    {
        CourseResponseModel model = new CourseResponseModel();
        CourseDataModel courseData = new CourseDataModel();
        try
        {
            var item = await _db.TblCourses.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CourseId == id);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Course Found", false);
                return model;
            }
            courseData.CourseId = item.CourseId;
            courseData.SubjectCode = item.SubjectCode;
            courseData.CourseName = item.CourseName;
            courseData.Description = item.Description;
            courseData.DueDate = item.DueDate;
            courseData.Status = item.Status;
            model.Course = courseData;
            model.Response = SubResponseModel.GetResponseMsg("Your course is successfully retrieved.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<CourseResponseModel> Update(CourseRequestModel reqModel)
    {
        CourseResponseModel model = new CourseResponseModel();
        try
        {
            TblCourse? item = await _db.TblCourses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CourseId == reqModel.CourseId && x.CreatedUserId == reqModel.CurrentUserId);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No course Found", false);
                return model;
            }

            item.SubjectCode = reqModel.SubjectCode;
            item.CourseName = reqModel.CourseName;
            item.Description = reqModel.Description;
            item.DueDate = reqModel.DueDate;
            item.Status = reqModel.Status;
            item.UpdatedDate = DateTime.Now;
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();
            model.Response = SubResponseModel.GetResponseMsg("Your course is successfully updated.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

}
