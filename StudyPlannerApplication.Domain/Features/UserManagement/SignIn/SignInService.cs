namespace StudyPlannerApplication.Domain.Features.UserManagement.SignIn;

public class SignInService
{
    private readonly AppDbContext _db;

    public SignInService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<SignInResponseModel>> SignIn(SignInRequestModel reqModel)
    {
        SignInResponseModel model = new SignInResponseModel();
        try
        {
            string hashPassword = reqModel.Password.ToSHA256HexHashString(reqModel.UserName);
            TblUser? user = await _db.TblUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == reqModel.UserName && x.Password == hashPassword);
            if (user is null)
            {
                return Result<SignInResponseModel>.FailureResult("UserName and Password is wrong");
            }
            model.UserName = user!.UserName;
            model.Phone = user.PhoneNo;
            model.UserId = user.UserId;
            model.Role = user.RoleCode;
            return Result<SignInResponseModel>.SuccessResult(model,"Welcome to your account.");
        }
        catch(Exception ex)
        {
            return Result<SignInResponseModel>.FailureResult(ex.ToString()); 
        }
    }
}
