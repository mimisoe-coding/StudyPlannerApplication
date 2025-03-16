namespace StudyPlannerApplication.Domain.Features.LiveChat;

public class LiveChatResponseModel
{
   public List<LiveChatDataModel> MessageList { get; set; }
}

public class LiveChatDataModel
{
    public string UserId { get; set; }
    public string Message { get; set; }
    public string ImageUrl { get; set; }
    public DateTime? CreatedDate { get; set; }
}
