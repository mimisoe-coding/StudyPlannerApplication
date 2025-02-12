namespace StudyPlannerApplication.Domain.Features.UserManagement.Login;

public class LogInService
{
    private readonly AppDbContext _db;

    public LogInService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<LoginResponseModel> LogIn(LogInRequestModel reqModel)
    {
        LoginResponseModel model = new LoginResponseModel();
        TblUser? user = await _db.TblUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == reqModel.UserName && x.Password == reqModel.Password);
        if(user is null)
        {
            model.Response = SubResponseModel.GetResponseMsg("UserName and Password is wrong", false);
            return model;
        }
        model.UserName = user!.UserName;
        model.Phone = user.PhoneNo;
        model.UserId = user.UserId;
        model.Role = user.RoleCode;
        model.Response = SubResponseModel.GetResponseMsg("Welcome to your account.", true);
        return model;
    }

}
