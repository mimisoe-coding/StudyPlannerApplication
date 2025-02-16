namespace StudyPlannerApplication.Domain.Features.Subject;

public class SubjectService
{
    private readonly AppDbContext _db;

    public SubjectService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SubjectResponseModel> Create(SubjectRequestModel reqModel)
    {
        SubjectResponseModel model = new SubjectResponseModel();
        try
        {
            #region Check Duplicate Subject

            var item = await _db.TblSubjects.AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectName.ToLower() == reqModel.SubjectName.ToLower());
            if (item is not null)
            {
                model.Response = SubResponseModel.GetResponseMsg("Your subject is already exists.", false);
                return model;
            }
            #endregion

            TblSubject subject = new TblSubject();
            subject.SubjectName = reqModel.SubjectName;
            subject.SubjectCode = GenerateCode(reqModel.SubjectName);
            subject.Description = reqModel.Description;
            subject.CreatedDate = DateTime.Now;

            await _db.AddAsync(subject);
            await _db.SaveAndDetachAsync();
            model.Response = SubResponseModel.GetResponseMsg("Your subject is successfully added.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<SubjectResponseModel> List(SubjectRequestModel reqModel)
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
            model.Response = SubResponseModel.GetResponseMsg("Your subjects are retrieved successfully.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<SubjectResponseModel> Delete(SubjectRequestModel reqModel)
    {
        SubjectResponseModel model = new SubjectResponseModel();
        try
        {
            var item = await _db.TblSubjects.AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectId == reqModel.SubjectId);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Subject Found", false);
                return model;
            }
            _db.Remove(item);
            await _db.SaveAndDetachAsync();
            model.Response = SubResponseModel.GetResponseMsg("Your subject is successfully deleted", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<SubjectResponseModel> Update(SubjectRequestModel reqModel)
    {
        SubjectResponseModel model = new SubjectResponseModel();
        try
        {
            TblSubject? item = await _db.TblSubjects
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectId == reqModel.SubjectId);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Subject Found", false);
                return model;
            }

            item.SubjectName = reqModel.SubjectName;
            item.Description = reqModel.Description;
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();
            model.Response = SubResponseModel.GetResponseMsg("Your subject is successfully updated.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<SubjectResponseModel> Edit(int id)
    {
        SubjectResponseModel model = new SubjectResponseModel();
        SubjectDataModel subjectData = new SubjectDataModel();
        try
        {
            var item = await _db.TblSubjects.AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectId == id);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Subject Found", false);
                return model;
            }
            subjectData.SubjectId = item.SubjectId;
            subjectData.SubjectCode = item.SubjectCode;
            subjectData.SubjectName = item.SubjectName;
            subjectData.Description = item.Description;
            model.Subject = subjectData;
            model.Response = SubResponseModel.GetResponseMsg("Your subject is successfully retrieved.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<SubjectResponseModel> GetSubjectList(int id)
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
            model.Response = SubResponseModel.GetResponseMsg("Your subject is successfully added.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }


    private string GenerateCode(string subjectName)
    {
        string prefix = subjectName.Trim().Substring(0, 3).ToUpper();

        Random rdn = new Random();
        string code = prefix + rdn.Next(100, 999).ToString();

        return code;
    }
}
