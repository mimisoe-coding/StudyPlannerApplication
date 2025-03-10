using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net;
using System.Net.Mail;

namespace StudyPlannerApplication.Domain.Features.UserManagement.ChangePassword;

public class ChangePasswordService
{
    private readonly AppDbContext _db;

    public ChangePasswordService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<ChangePasswordResponseModel>> ResetPassword(ChangePasswordRequestModel reqModel)
    {
        ChangePasswordResponseModel model = new ChangePasswordResponseModel();
        try
        {
            var user = await _db.TblUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Email == reqModel.Email);
            if (user is null)
            {
                return Result<ChangePasswordResponseModel>.FailureResult("Invalid email.");
            }

            string password = DevCode.GeneratePassword();
            string hashPassword = password.ToSHA256HexHashString(user.UserName);
            user.Password = hashPassword;
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();

            #region Send Email

            var senderEmail = "studyplannerhub@gmail.com";
            var senderPassword = "nhyr ysyd owwk jama";
            var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                Port = 587
            });

            Email.DefaultSender = sender;

            var emailBody = $@"
                                Your password has been successfully reset.
                                UserName: {user.UserName}
                                Password: {password}";

            var email = await Email
                .From(senderEmail)
                .To(user.Email, user.UserName)
                .Subject("Password Reset Successful")
                .Body(emailBody, false)
                .SendAsync();

            if (!email.Successful)
                return Result<ChangePasswordResponseModel>.FailureResult("Sending email is fail");

            return Result<ChangePasswordResponseModel>.SuccessResult("Sending email is successful");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ChangePasswordResponseModel>.FailureResult(ex.ToString());
        }
    }
}
