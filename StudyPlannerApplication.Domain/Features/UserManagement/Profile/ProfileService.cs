namespace StudyPlannerApplication.Domain.Features.UserManagement.Profile;

public class ProfileService
{
    private readonly AppDbContext _db;
    public ProfileService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ProfileResponseModel> Profile(ProfileRequestModel reqModel)
    {
        ProfileResponseModel model = new ProfileResponseModel();
        try
        {
            var user = await _db.TblUsers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId==reqModel.UserId);
            if (user == null)
            {
                model.Response = SubResponseModel.GetResponseMsg("No User Found", false);
                return model;
            }
            model.UserId = user.UserId;
            model.UserName = user.UserName;
            model.Phone = user.PhoneNo;
            model.Email = user.Email;
            model.Response = SubResponseModel.GetResponseMsg("Your profile is retrieved.", true);
        }catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }
}
