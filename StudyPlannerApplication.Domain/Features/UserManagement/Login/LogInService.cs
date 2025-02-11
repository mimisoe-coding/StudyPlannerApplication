using Microsoft.EntityFrameworkCore;
using StudyPlannerApplication.Database.EFAppDbContextModels;

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
        var user = await _db.TblUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == reqModel.UserName && x.Password == reqModel.Password);
        if(user is not null)
        {
            model.UserName = user.UserName;
            model.Phone = user.PhoneNo;
        }
        return model;
    }
}
