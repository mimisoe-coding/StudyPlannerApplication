using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using StudyPlannerApplication.Domain.Features.UserManagement.Profile;
using StudyPlannerApplication.Domain.Hubs;
using System.Dynamic;

namespace StudyPlannerApplication.App.Components.Pages.Chat;

public partial class P_Chat
{
    //private string? user = "admin";
    private string? message;
    private string LiveChatGroupId;
    private HubConnection? hubConnection;
    private UserSessionModel _userSession = new();
    private List<LiveChatRequestModel> allMessages = new List<LiveChatRequestModel>();
    private ProfileRequestModel _reqModel = new();
    private Result<ProfileResponseModel> _resModel = new();
    private string _imageBase64Str = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        //hubConnection = new HubConnectionBuilder()
        //    .WithUrl("https://localhost:7261/liveChatHub")
        //    .Build();

        //hubConnection.On<LiveChatRequestModel>("AdminReceiveMessage", (item) =>
        //{
        //    LiveChatGroupId = item.LiveChatGroupId;
        //    messages.Add(item);
        //    InvokeAsync(StateHasChanged);
        //});

        //await hubConnection.StartAsync();

        //await hubConnection.SendAsync("Connect", _userSession.UserId);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
            var authState = await customAuthStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity != null && !authState.User.Identity.IsAuthenticated)
            {
                _nav.NavigateTo("/");
                return;
            }
            _userSession = await customAuthStateProvider.GetUserData();
            await GetAllMessage();
            await UserData();
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7261/liveChatHub")
                .Build();

            hubConnection.On<LiveChatRequestModel>("AdminReceiveMessage", async (item) =>
            {
                LiveChatGroupId = item.LiveChatGroupId;
                allMessages.Add(item);
                allMessages = allMessages.OrderBy(x => x.CreatedDate).ToList();
                await InvokeAsync(StateHasChanged);
            });

            await hubConnection.StartAsync();

            await hubConnection.SendAsync("Connect", _userSession.UserId);
            await InvokeAsync(StateHasChanged);
        }

    }

    private async Task Send()
    { 
        if(string.IsNullOrWhiteSpace(message) || string.IsNullOrEmpty(message))
            return;
        if (hubConnection is not null)
        {
            var item = new LiveChatRequestModel
            {
                LiveChatGroupId = LiveChatGroupId,
                UserName = _userSession.UserId,
                CreatedDate = DateTime.Now,
                Message = message,
                ImageUrl = _resModel.Data.ImagePath
            };
            allMessages.Add(item);
            await hubConnection.SendAsync("AdminSendMessage", item);
            await InvokeAsync(StateHasChanged);
        }
        message = string.Empty;
    }

    public bool isConnected =>
        hubConnection?.State == HubConnectionState.Connected;

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !string.IsNullOrWhiteSpace(message))
        {
            await Send();
        }
    }

    private async Task UserData()
    {
        try
        {
            _reqModel.UserId = _userSession.UserId;
            _resModel = await _profileService.Profile(_reqModel);
            if (!_resModel.Success)
            {
                await _injectService.ErrorMessage(_resModel.Message);
                return;
            }
            _imageBase64Str = "data:image;base64," + _resModel.Data.ImageStr;
            if (_resModel.Data.ImagePath is null)
            {
                _imageBase64Str = "images/profile/profile.png";
            }
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    private ChatBubblePosition ChangePosition(string userId)
    {
        if (userId == _userSession.UserId)
        {
            return ChatBubblePosition.End;
        }
        else
        {
            return ChatBubblePosition.Start;
        }
    }

    private string GetImage64(string image)
    {
        if (image is null)
        {
            return "images/profile/profile.png";
        }
        byte[] imageBytes = File.ReadAllBytes(image);
        string base64String = Convert.ToBase64String(imageBytes);

        return $"data:image/png;base64,{base64String}";
    }

    private async Task GetAllMessage()
    {
        try
        {
            var result = await _liveChatService.GetMessage();
            if (!result.Success)
            {
                await _injectService.ErrorMessage(result.Message);
                return;
            }
            allMessages = result.Data.MessageList.Select(x=>new LiveChatRequestModel
            {
                CreatedDate = Convert.ToDateTime(x.CreatedDate),
                Message = x.Message,
                ImageUrl = x.ImageUrl,
                UserName = x.UserId
            }).OrderBy(x=>x.CreatedDate).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    private async Task SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            await Send();
        }
    }
}
