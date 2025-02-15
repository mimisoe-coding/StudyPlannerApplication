namespace StudyPlannerApplication.Domain.Features.UserManagement.SignIn;

public class SignInService
{
    private readonly AppDbContext _db;

    public SignInService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SignInResponseModel> SignIn(SignInRequestModel reqModel)
    {
        SignInResponseModel model = new SignInResponseModel();
        try
        {
            string hashPassword = reqModel.Password.ToSHA256HexHashString(reqModel.UserName);
            TblUser? user = await _db.TblUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == reqModel.UserName && x.Password == hashPassword);
            if (user is null)
            {
                model.Response = SubResponseModel.GetResponseMsg("UserName and Password is wrong", false);
                return model;
            }
            model.UserName = user!.UserName;
            model.Phone = user.PhoneNo;
            model.UserId = user.UserId;
            model.Role = user.RoleCode;
            model.Response = SubResponseModel.GetResponseMsg("Welcome to your account.", true);
        }
        catch(Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false); 
        }
        
        return model;
    }

}
