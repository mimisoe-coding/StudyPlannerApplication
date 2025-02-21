using System;
using System.Collections.Generic;

namespace StudyPlannerApplication.Database.EFAppDbContextModels;

public partial class TblNotification
{
    public int NotificationId { get; set; }

    public int ExamId { get; set; }

    public string UserId { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
}
