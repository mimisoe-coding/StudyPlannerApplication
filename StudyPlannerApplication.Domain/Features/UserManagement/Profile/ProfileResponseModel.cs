namespace StudyPlannerApplication.Domain.Features.UserManagement.Profile;

public class ProfileResponseModel
{
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public ResponseModel Response { get; set; }
}
