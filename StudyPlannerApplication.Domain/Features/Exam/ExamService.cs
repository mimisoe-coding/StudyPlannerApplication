using StudyPlannerApplication.Domain.Features.Subject;

namespace StudyPlannerApplication.Domain.Features.Exam;

public class ExamService
{
    private readonly AppDbContext _db;

    public ExamService(AppDbContext db)
    {
        _db = db;
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
            exam.Duration =reqModel.Duration;
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
}
