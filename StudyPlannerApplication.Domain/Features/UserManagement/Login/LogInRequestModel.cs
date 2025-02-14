using System.ComponentModel.DataAnnotations;

namespace StudyPlannerApplication.Domain.Features.UserManagement.Login;

public class LogInRequestModel
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string PhoneNo { get; set; }
    public string Email { get; set; }
    public string ConfirmPassword { get; set; } 
}
