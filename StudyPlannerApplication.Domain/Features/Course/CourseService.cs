namespace StudyPlannerApplication.Domain.Features.Course;

public class CourseService
{
    private readonly AppDbContext _db;

    public CourseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<CourseResponseModel>> Create(CourseRequestModel reqModel)
    {
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
            return Result<CourseResponseModel>.SuccessResult("Your course is successfully added.");
        }
        catch (Exception ex)
        {
            return Result<CourseResponseModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<CourseResponseModel>> List(CourseRequestModel reqModel)
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
            return Result<CourseResponseModel>.SuccessResult(model, "Your course is successfully added.");
        }
        catch (Exception ex)
        {
            return Result<CourseResponseModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<CourseResponseModel>> Delete(CourseRequestModel reqModel)
    {
        try
        {
            var item = await _db.TblCourses.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CourseId == reqModel.CourseId
                && x.CreatedUserId == reqModel.CurrentUserId);
            if (item == null)
            {
                return Result<CourseResponseModel>.FailureResult("No Course Found");
            }
            _db.Remove(item);
            await _db.SaveChangesAsync();
            return Result<CourseResponseModel>.SuccessResult("Your course is successfully deleted");
        }
        catch (Exception ex)
        {
            return Result<CourseResponseModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<CourseResponseModel>> Edit(int id)
    {
        CourseResponseModel model = new CourseResponseModel();
        CourseDataModel courseData = new CourseDataModel();
        try
        {
            var item = await _db.TblCourses.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CourseId == id);
            if (item == null)
            {
                return Result<CourseResponseModel>.FailureResult("No Course Found");
            }
            courseData.CourseId = item.CourseId;
            courseData.SubjectCode = item.SubjectCode;
            courseData.CourseName = item.CourseName;
            courseData.Description = item.Description;
            courseData.DueDate = item.DueDate;
            courseData.Status = item.Status;
            model.Course = courseData;
            return Result<CourseResponseModel>.SuccessResult(model, "Your course is successfully retrieved.");
        }
        catch (Exception ex)
        {
            return Result<CourseResponseModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<CourseResponseModel>> Update(CourseRequestModel reqModel)
    {
        try
        {
            TblCourse? item = await _db.TblCourses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CourseId == reqModel.CourseId && x.CreatedUserId == reqModel.CurrentUserId);
            if (item == null)
            {
                return Result<CourseResponseModel>.FailureResult("No course Found");
            }

            item.SubjectCode = reqModel.SubjectCode;
            item.CourseName = reqModel.CourseName;
            item.Description = reqModel.Description;
            item.DueDate = reqModel.DueDate;
            item.Status = reqModel.Status;
            item.UpdatedDate = DateTime.Now;
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();
            return Result<CourseResponseModel>.SuccessResult("Your course is successfully updated.");
        }
        catch (Exception ex)
        {
            return Result<CourseResponseModel>.FailureResult(ex.ToString());
        }
    }
}
