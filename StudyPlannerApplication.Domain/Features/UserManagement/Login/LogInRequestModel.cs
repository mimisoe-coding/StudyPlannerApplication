using System.ComponentModel.DataAnnotations;

namespace StudyPlannerApplication.Domain.Features.UserManagement.Login;

public class LogInRequestModel
{
    [Required(ErrorMessage = "UserName Field is required.")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Password Field is required.")]
    public string? Password { get; set; }
}
