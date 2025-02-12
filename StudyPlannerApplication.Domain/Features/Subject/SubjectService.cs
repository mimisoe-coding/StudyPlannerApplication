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
            subject.CreatedUserId = reqModel.CurrentUserId;
            subject.Description = reqModel.Description;
            subject.CreatedDate = DateTime.Now;

            await _db.AddAsync(subject);
            await _db.SaveChangesAsync();
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
