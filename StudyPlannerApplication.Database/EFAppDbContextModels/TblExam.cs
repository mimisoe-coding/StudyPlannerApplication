using System;
using System.Collections.Generic;

namespace StudyPlannerApplication.Database.EFAppDbContextModels;

public partial class TblExam
{
    public int ExamId { get; set; }

    public string SubjectCode { get; set; } = null!;

    public string? Description { get; set; }

    public TimeOnly Duration { get; set; }

    public DateTime DueDate { get; set; }

    public string Status { get; set; } = null!;

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
