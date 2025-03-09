namespace StudyPlannerApplication.Domain.Features.Subject;

public class SubjectService
{
    private readonly AppDbContext _db;

    public SubjectService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<SubjectResponseModel>> Create(SubjectRequestModel reqModel)
    {
        SubjectResponseModel model = new SubjectResponseModel();
        try
        {
            #region Check Duplicate Subject

            var item = await _db.TblSubjects.AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectName.ToLower() == reqModel.SubjectName.ToLower());
            if (item is not null)
            {
                return Result<SubjectResponseModel>.FailureResult("Your subject is already exists.");
            }
            #endregion

            TblSubject subject = new TblSubject();
            subject.SubjectName = reqModel.SubjectName;
            subject.SubjectCode = GenerateCode(reqModel.SubjectName);
            subject.Description = reqModel.Description;
            subject.CreatedDate = DateTime.Now;

            await _db.AddAsync(subject);
            await _db.SaveAndDetachAsync();
            return Result<SubjectResponseModel>.SuccessResult("Your subject is successfully added.");
        }
        catch (Exception ex)
        {
            return Result<SubjectResponseModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<SubjectResponseModel>> List(SubjectRequestModel reqModel)
    {
        SubjectResponseModel model = new SubjectResponseModel();
        PageSettingResponseModel pageSetting = new();
        try
        {
            var lst = await _db.TblSubjects.AsNoTracking().
                Select(x => new SubjectDataModel
                {
                    SubjectId = x.SubjectId,
                    SubjectCode = x.SubjectCode,
                    SubjectName = x.SubjectName,
                    Description = x.Description
                }).ToListAsync();
            pageSetting.TotalRowCount = lst.Count;
            model.PageSetting = pageSetting;
            model.SubjectList = lst.Skip(reqModel.PageSetting.SkipRowCount)
                .Take(reqModel.PageSetting.PageSize).ToList();
            return Result<SubjectResponseModel>.SuccessResult(model, "Your subjects are retrieved successfully.");
        }
        catch (Exception ex)
        {
            return Result<SubjectResponseModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<SubjectResponseModel>> Delete(SubjectRequestModel reqModel)
    {
        try
        {
            var item = await _db.TblSubjects.AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectId == reqModel.SubjectId);
            if (item == null)
            {
                return Result<SubjectResponseModel>.FailureResult("No Subject Found");
            }
            _db.Remove(item);
            await _db.SaveAndDetachAsync();
            return Result<SubjectResponseModel>.SuccessResult("Your subject is successfully deleted");
        }
        catch (Exception ex)
        {
            return Result<SubjectResponseModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<SubjectResponseModel>> Update(SubjectRequestModel reqModel)
    {
        try
        {
            TblSubject? item = await _db.TblSubjects
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectId == reqModel.SubjectId);
            if (item == null)
            {
                return Result<SubjectResponseModel>.FailureResult("No Subject Found");
            }

            item.SubjectName = reqModel.SubjectName;
            item.Description = reqModel.Description;
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();
            return Result<SubjectResponseModel>.SuccessResult("Your subject is successfully updated.");
        }
        catch (Exception ex)
        {
            return Result<SubjectResponseModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<SubjectResponseModel>> Edit(int id)
    {
        SubjectResponseModel model = new SubjectResponseModel();
        SubjectDataModel subjectData = new SubjectDataModel();
        try
        {
            var item = await _db.TblSubjects.AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectId == id);
            if (item == null)
            {
                return Result<SubjectResponseModel>.FailureResult("No Subject Found");
            }
            subjectData.SubjectId = item.SubjectId;
            subjectData.SubjectCode = item.SubjectCode;
            subjectData.SubjectName = item.SubjectName;
            subjectData.Description = item.Description;
            model.Subject = subjectData;
            return Result<SubjectResponseModel>.SuccessResult(model, "Your subject is successfully retrieved.");
        }
        catch (Exception ex)
        {
            return Result<SubjectResponseModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<SubjectResponseModel>> GetSubjectList(string userId)
    {
        SubjectResponseModel model = new SubjectResponseModel();
        PageSettingResponseModel pageSetting = new();
        try
        {
            var lst = await _db.TblSubjects.AsNoTracking().
                Select(x => new SubjectDataModel
                {
                    SubjectId = x.SubjectId,
                    SubjectCode = x.SubjectCode,
                    SubjectName = x.SubjectName
                }).ToListAsync();
            model.SubjectList = lst;
            return Result<SubjectResponseModel>.SuccessResult(model, "Your subject is successfully added.");
        }
        catch (Exception ex)
        {
            return Result<SubjectResponseModel>.FailureResult(ex.ToString());
        }
    }


    private string GenerateCode(string subjectName)
    {
        string prefix = subjectName.Trim().Substring(0, 3).ToUpper();

        Random rdn = new Random();
        string code = prefix + rdn.Next(100, 999).ToString();

        return code;
    }
}
