using System;
using System.Collections.Generic;

namespace StudyPlannerApplication.Database.EFAppDbContextModels;

public partial class TblLiveChatLogin
{
    public string LiveChatLoginId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string ConnectionId { get; set; } = null!;
}
