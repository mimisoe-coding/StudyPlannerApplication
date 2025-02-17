using System;
using System.Collections.Generic;

namespace StudyPlannerApplication.Database.EFAppDbContextModels;

public partial class TblCourse
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public string SubjectCode { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public DateTime DueDate { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
