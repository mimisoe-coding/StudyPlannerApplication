using FluentEmail.Core;
using FluentEmail.Smtp;
using StudyPlannerApplication.Domain.Features.UserManagement.Profile;
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

    public async Task<ChangePasswordResponseModel> ResetPassword(ChangePasswordRequestModel reqModel)
    {
        ChangePasswordResponseModel model = new ChangePasswordResponseModel();
        try
        {
            var senderEmail ="studyplannerhub@gmail.com";
            var password = "nhyr ysyd owwk jama";
            var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail,password),
                EnableSsl = true,
                Port = 587
            });

            Email.DefaultSender = sender;

            var email = await Email
                .From(senderEmail)
                .To("mimisoe968@gmail.com", "Mi Mi Soe")
                .Subject("Test Email")
                .Body("This is a test email sent using FluentEmail.", false)
                .SendAsync();

            if (email.Successful)
            {
                Console.WriteLine("Email sent successfully!");
            }
            else
            {
                Console.WriteLine("Failed to send email. Errors: " + string.Join(", ", email.ErrorMessages));
            }
        }
        catch (Exception ex)
        {
            model.Response = SubResponseModel.GetResponseMsg(ex.ToString(), false);
        }
        return model;
    }
}
