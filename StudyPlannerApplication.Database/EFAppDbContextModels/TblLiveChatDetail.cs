using System;
using System.Collections.Generic;

namespace StudyPlannerApplication.Database.EFAppDbContextModels;

public partial class TblLiveChatDetail
{
    public string LiveChatDetailId { get; set; } = null!;

    public string? LiveChatGroupId { get; set; }

    public string? LiveChatUserId { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public string? ImagePath { get; set; }
}
