using System;
using System.Collections.Generic;

namespace StudyPlannerApplication.Database.EFAppDbContextModels;

public partial class TblUser
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNo { get; set; } = null!;

    public string? ImagePath { get; set; }

    public string RoleCode { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
