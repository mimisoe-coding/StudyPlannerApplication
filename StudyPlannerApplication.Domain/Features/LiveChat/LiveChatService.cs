namespace StudyPlannerApplication.Domain.Features.LiveChat;

public class LiveChatService
{
    private readonly AppDbContext _db;

    public LiveChatService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<LiveChatResponseModel>> GetMessage()
    {
        LiveChatResponseModel model = new LiveChatResponseModel();
        PageSettingResponseModel pageSetting = new();
        try
        {
            var lst = await _db.TblLiveChatDetails.AsNoTracking().
                Select(x => new LiveChatDataModel
                {
                    UserId = x.LiveChatUserId,
                    Message = x.Message,
                    ImageUrl = x.ImagePath,
                    CreatedDate = x.CreatedDateTime
                }).ToListAsync();
            
            model.MessageList = lst;
            return Result<LiveChatResponseModel>.SuccessResult(model, "Message are successfully retrieved.");
        }
        catch (Exception ex)
        {
            return Result<LiveChatResponseModel>.FailureResult(ex.ToString());
        }
    }
}
