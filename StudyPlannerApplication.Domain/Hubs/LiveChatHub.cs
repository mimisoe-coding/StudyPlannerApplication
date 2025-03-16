using Microsoft.AspNetCore.SignalR;

namespace StudyPlannerApplication.Domain.Hubs;

public class LiveChatHub : Hub
{
    private readonly AppDbContext _db;

    public LiveChatHub(AppDbContext db)
    {
        _db = db;
    }

    private string ConnectionId
    {
        get
        {
            return Context.ConnectionId;
        }
    }

    public async Task Connect(string userId)
    {
        try
        {
            TblLiveChatLogin liveChatLogin = new TblLiveChatLogin();
            liveChatLogin.LiveChatLoginId = Guid.NewGuid().ToString();
            liveChatLogin.UserId = userId;
            liveChatLogin.ConnectionId = ConnectionId;
            await _db.TblLiveChatLogins.AddAsync(liveChatLogin);
            var result = await _db.SaveChangesAsync();
            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public async Task AdminSendMessage(LiveChatRequestModel reqModel)
    {
        if (await SaveLiveChatLoginDetail(reqModel))
        {
            await Clients.AllExcept(ConnectionId)
                .SendAsync("AdminReceiveMessage", reqModel);
        }
    }

    //public async Task ClientSendMessage(LiveChatRequestModel reqModel)
    //{
    //    if (await SaveLiveChatLoginDetail(reqModel))
    //    {
    //        var data = await _db.TblLiveChatLogins
    //                .FirstOrDefaultAsync(x => x.UserId == "admin");
    //        await Clients.Client(data!.ConnectionId)
    //            .SendAsync("AdminReceiveMessage", reqModel);
    //    }
    //}

    private async Task<bool> SaveLiveChatLoginDetail(LiveChatRequestModel reqModel)
    {
        TblLiveChatDetail liveChatDetail = new TblLiveChatDetail();
        liveChatDetail.LiveChatDetailId = Guid.NewGuid().ToString();
        liveChatDetail.LiveChatGroupId = reqModel.LiveChatGroupId;
        liveChatDetail.LiveChatUserId = reqModel.UserName;
        liveChatDetail.ImagePath = reqModel.ImageUrl;
        liveChatDetail.Message = reqModel.Message;
        liveChatDetail.CreatedDateTime = DateTime.Now;
        await _db.TblLiveChatDetails.AddAsync(liveChatDetail);
        var result = await _db.SaveChangesAsync();
        if (result > 0) return true;
        return false;
    }
}
