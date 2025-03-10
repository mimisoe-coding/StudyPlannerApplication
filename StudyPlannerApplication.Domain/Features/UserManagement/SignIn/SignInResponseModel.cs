namespace StudyPlannerApplication.Domain.Features.UserManagement.Login;

public class SignInResponseModel { 
    public string UserId { get; set; }
    public string? UserName { get; set; }
    public string? Phone { get; set; }
    public string? Role { get; set; }
}
