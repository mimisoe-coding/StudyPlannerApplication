using StudyPlannerApplication.Shared.Enums;

namespace StudyPlannerApplication.Domain.Features.UserManagement.UserRegistration;

public class RegisterService
{
    private readonly AppDbContext _db;

    public RegisterService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<SignInResponseModel>> Register(SignInRequestModel reqModel)
    {
        try
        {
            #region Check Duplicate UserName and PhoneNo

            TblUser? user = await _db.TblUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserName.ToLower().Trim() == reqModel.UserName.ToLower().Trim()
            || x.PhoneNo==reqModel.PhoneNo || x.Email.ToLower()==reqModel.Email.ToLower());
            if (user is not null)
            {
                if (user.UserName.ToLower().Trim() == reqModel.UserName.ToLower().Trim())
                {
                    return Result<SignInResponseModel>.FailureResult("Your UserName is already used.");
                }
                if (user.PhoneNo == reqModel.PhoneNo)
                {
                    return Result<SignInResponseModel>.FailureResult("Your PhoneNo is already used.");
                }
                if (user.Email.ToLower() == reqModel.Email.ToLower())
                {
                    return Result<SignInResponseModel>.FailureResult("Your Email is already used.");
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
            return Result<SignInResponseModel>.SuccessResult("Registration Success!Welcome to your account.");
        }
        catch (Exception ex)
        {
            return Result<SignInResponseModel>.FailureResult(ex.ToString());
        }
    }
}
