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
                .FirstOrDefaultAsync(x => x.UserId == reqModel.UserId);
            if (user == null)
            {
                return Result<ProfileResponseModel>.FailureResult("No User Found");
            }
            model.UserId = user.UserId;
            model.UserName = user.UserName;
            model.Phone = user.PhoneNo;
            model.Email = user.Email;
            model.ImagePath = user.ImagePath;
            if (System.IO.File.Exists(model.ImagePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(model.ImagePath);
                model.ImageStr = Convert.ToBase64String(imageBytes);
            }

            return Result<ProfileResponseModel>.SuccessResult(model, "Your profile is retrieved.");
        }
        catch (Exception ex)
        {
            return Result<ProfileResponseModel>.FailureResult(ex.ToString());
        }
    }

    public async Task<Result<ProfileResponseModel>> Save(ProfileRequestModel reqModel)
    {
        ProfileResponseModel model = new ProfileResponseModel();
        try
        {
            var user = await _db.TblUsers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == reqModel.UserId);
            if (user == null)
            {
                return Result<ProfileResponseModel>.FailureResult("No User Found");
            }

            #region Write Image File

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\STP_App\\profile");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}{reqModel.ImageExtension}";
            var filePath = Path.Combine(folderPath, fileName);

            var fileData = Convert.FromBase64String(reqModel.ImageFile);
            await File.WriteAllBytesAsync(filePath, fileData);

            #endregion

            #region Save Image Path

            user.ImagePath = filePath;
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            #endregion

            return Result<ProfileResponseModel>.SuccessResult(model, "Your profile is successfully saved.");
        }
        catch (Exception ex)
        {
            return Result<ProfileResponseModel>.FailureResult(ex.ToString());
        }
    }
}
