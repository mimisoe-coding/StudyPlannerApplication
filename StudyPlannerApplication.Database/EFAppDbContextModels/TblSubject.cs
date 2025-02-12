using System;
using System.Collections.Generic;

namespace StudyPlannerApplication.Database.EFAppDbContextModels;

public partial class TblSubject
{
    public int SubjectId { get; set; }

    public string SubjectCode { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public string? Description { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
