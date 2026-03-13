namespace StudyPlannerApplication.App.Components.Pages.Dashboard;

public partial class P_Dashboard
{
    private UserSessionModel _userSession = new();
    private DashboardRequestModel _reqModel = new();
    private Result<DashboardResponseModel> _resModel = new();

    [Inject] private PageHeaderService HeaderService { get; set; }

    protected override void OnInitialized()
    {
        HeaderService.SetHeader(
            "Dashboard",
            "Welcome back! Here's what's on your schedule.",
            "#3B82F6",
            "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\"><rect width=\"18\" height=\"18\" x=\"3\" y=\"4\" rx=\"2\" ry=\"2\"/><line x1=\"16\" x2=\"16\" y1=\"2\" y2=\"6\"/><line x1=\"8\" x2=\"8\" y1=\"2\" y2=\"6\"/><line x1=\"3\" x2=\"21\" y1=\"10\" y2=\"10\"/><path d=\"M8 14h.01\"/><path d=\"M12 14h.01\"/><path d=\"M16 14h.01\"/><path d=\"M8 18h.01\"/><path d=\"M12 18h.01\"/><path d=\"M16 18h.01\"/></svg>"
        );
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
            //await Notification();
            await GetCourseList();
            StateHasChanged();
        }
    }

    //async Task Notification()
    //{
    //    NotificationRequestModel reqModel = new NotificationRequestModel
    //    {
    //        CurrentUserId = _userSession.UserId
    //    };
    //    var notiData = await _notificationService.GetAllNotification(reqModel);
    //    if (!notiData.Response.IsSuccess)
    //    {
    //        await _injectService.ErrorMessage(notiData.Response.Message);
    //        return;
    //    }

    //    _notificationStateContainer.NotificationCount = notiData.NotiList.Count;
    //}

    async Task GetCourseList()
    {
        _reqModel.CurrentUserId = _userSession.UserId;
        _resModel = await _dashboardService.GetAllCourseList(_reqModel);
        if (!_resModel.Success)
        {
            await _injectService.ErrorMessage(_resModel.Message);
        }
    }


}
