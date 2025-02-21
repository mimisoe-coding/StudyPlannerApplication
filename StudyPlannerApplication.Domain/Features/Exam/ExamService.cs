using System;

namespace StudyPlannerApplication.Domain.Features.Exam;

public class ExamService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapper;

    public ExamService(AppDbContext db, DapperService dapper)
    {
        _db = db;
        _dapper = dapper;
    }

    public async Task<ExamResponseModel> Create(ExamRequestModel reqModel)
    {
        ExamResponseModel model = new ExamResponseModel();
        try
        {
            #region Check Duplicate Subject

            var item = await _db.TblExams.AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectCode.ToLower() == reqModel.SubjectCode.ToLower());
            if (item is not null)
            {
                model.Response = SubResponseModel.GetResponseMsg("Your exam subject is already exists.", false);
                return model;
            }
            #endregion

            TblExam exam = new TblExam();
            exam.SubjectCode = reqModel.SubjectCode;
            exam.Description = reqModel.Description;
            exam.Status = reqModel.Status;
            exam.Duration = TimeOnly.FromDateTime(reqModel.Duration); 
            exam.DueDate = reqModel.DueDate;
            exam.CreatedUserId = reqModel.CurrentUserId;
            exam.CreatedDate = DateTime.Now;

            await _db.AddAsync(exam);
            await _db.SaveAndDetachAsync();
            model.Response = SubResponseModel.GetResponseMsg("Your subject is successfully added.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<ExamResponseModel> List(ExamRequestModel reqModel)
    {
        ExamResponseModel model = new ExamResponseModel();
        PageSettingResponseModel pageSetting = new();
        try
        {
            string query = @"select e.ExamId,
e.SubjectCode,
e.Status,
e.Description,
e.Duration As DurationTime,
e.DueDate,
s.SubjectName  from
Tbl_Exam e inner join Tbl_Subject s
on e.SubjectCode =s.SubjectCode
where e.CreatedUserId = @CurrentUserId";
            var result = _dapper.Query<ExamDataModel>(query, reqModel);
      
            var lst = result.ToList();

            pageSetting.TotalRowCount = lst.Count;
            model.PageSetting = pageSetting;
            model.ExamList = lst.Skip(reqModel.PageSetting.SkipRowCount)
                .Take(reqModel.PageSetting.PageSize).ToList();
            model.Response = SubResponseModel.GetResponseMsg("Your Exam is successfully added.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<ExamResponseModel> Delete(ExamRequestModel reqModel)
    {
        ExamResponseModel model = new ExamResponseModel();
        try
        {
            var item = await _db.TblExams.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExamId == reqModel.ExamId
                && x.CreatedUserId == reqModel.CurrentUserId);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Exam Found", false);
                return model;
            }
            _db.Remove(item);
            await _db.SaveChangesAsync();
            model.Response = SubResponseModel.GetResponseMsg("Your Exam is successfully deleted", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<ExamResponseModel> Edit(int id)
    {
        ExamResponseModel model = new ExamResponseModel();
        ExamDataModel exam = new ExamDataModel();
        try
        {
            var item = await _db.TblExams.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExamId == id);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Exam Found", false);
                return model;
            }
            exam.ExamId = item.ExamId;
            exam.SubjectCode = item.SubjectCode;
            //ExamData.SubjectName = item.ExamName;
            exam.Description = item.Description;
            exam.Duration = DateTime.Today.Add(item.Duration.ToTimeSpan());
            exam.DueDate = item.DueDate;
            exam.Status = item.Status;
            model.ExamData = exam;
            model.Response = SubResponseModel.GetResponseMsg("Your Exam is successfully retrieved.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }

    public async Task<ExamResponseModel> Update(ExamRequestModel reqModel)
    {
        ExamResponseModel model = new ExamResponseModel();
        try
        {
            TblExam? item = await _db.TblExams
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExamId == reqModel.ExamId && x.CreatedUserId == reqModel.CurrentUserId);
            if (item == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No Exam Found", false);
                return model;
            }

            item.SubjectCode = reqModel.SubjectCode;
            item.Description = reqModel.Description;
            item.Duration = TimeOnly.FromDateTime(reqModel.Duration);
            item.DueDate = reqModel.DueDate;
            item.Status = reqModel.Status;
            item.UpdatedDate = DateTime.Now;
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();
            model.Response = SubResponseModel.GetResponseMsg("Your Exam is successfully updated.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }
}
