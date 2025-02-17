using StudyPlannerApplication.Shared.Enums;

namespace StudyPlannerApplication.Domain.Features.UserManagement.UserRegistration;

public class RegisterService
{
    private readonly AppDbContext _db;

    public RegisterService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SignInResponseModel> Register(SignInRequestModel reqModel)
    {
        SignInResponseModel model = new SignInResponseModel();
        try
        {
            #region Check Duplicate UserName and PhoneNo

            TblUser? user = await _db.TblUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserName.ToLower().Trim() == reqModel.UserName.ToLower().Trim()
            || x.PhoneNo==reqModel.PhoneNo);
            if (user is not null)
            {
                if (user.UserName.ToLower().Trim() == reqModel.UserName.ToLower().Trim())
                {
                    model.Response = SubResponseModel.GetResponseMsg("Your UserName is already used.", false);
                    return model;
                }
                if (user.PhoneNo == reqModel.PhoneNo)
                {
                    model.Response = SubResponseModel.GetResponseMsg("Your PhoneNo is already used.", false);
                    return model;
                }
            }

            #endregion

            string hashPassword =
                   reqModel.Password.ToSHA256HexHashString(reqModel.UserName);
            TblUser item = new TblUser();
            item.UserId = System.Guid.NewGuid().ToString();
            item.UserName = reqModel.UserName;
            item.PhoneNo = reqModel.PhoneNo;
            item.Email = reqModel.Email;
            item.Password = hashPassword;
            item.CreatedDate = DateTime.Now;
            item.RoleCode = EnumRoleType.Student.ToEnumDescription();
            await _db.AddAsync(item);
            await _db.SaveChangesAsync();
            model.Response = SubResponseModel.GetResponseMsg("Registration Success!Welcome to your account.", true);
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }

        return model;
    }
}
