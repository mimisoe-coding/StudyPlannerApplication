using StudyPlannerApplication.Domain.Features.Common;

namespace StudyPlannerApplication.Domain.Features.UserManagement.Login;

public class LoginResponseModel { 
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? Phone { get; set; }
    public string? Role { get; set; }
    public ResponseModel Response { get; set; }
}
