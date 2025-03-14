﻿namespace StudyPlannerApplication.Domain.Features.UserManagement.Profile;

public class ProfileResponseModel
{
    public string UserId { get; set; }
    public string? UserName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? ImagePath { get; set; }
    public byte[] ImageBytes { get; set; }
    public string? ImageStr { get; set; }
}
