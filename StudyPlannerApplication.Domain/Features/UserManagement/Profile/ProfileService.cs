namespace StudyPlannerApplication.Domain.Features.UserManagement.Profile;

public class ProfileService
{
    private readonly AppDbContext _db;
    public ProfileService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<ProfileResponseModel>> Profile(ProfileRequestModel reqModel)
    {
        ProfileResponseModel model = new ProfileResponseModel();
        try
        {
            var user = await _db.TblUsers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId==reqModel.UserId);
            if (user == null)
            {
                return Result<ProfileResponseModel>.FailureResult("No User Found");
            }
            model.UserId = user.UserId;
            model.UserName = user.UserName;
            model.Phone = user.PhoneNo;
            model.Email = user.Email;
            return Result<ProfileResponseModel>.SuccessResult(model,"Your profile is retrieved.");
        }catch (Exception ex)
        {
            return Result<ProfileResponseModel>.FailureResult(ex.ToString());
        }
    }
}
