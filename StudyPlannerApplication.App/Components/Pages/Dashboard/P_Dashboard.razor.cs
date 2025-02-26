using MudBlazor;
using StudyPlannerApplication.App.StateContainer;
using StudyPlannerApplication.Domain.Features.Dashboard;
using StudyPlannerApplication.Domain.Features.Notification;

namespace StudyPlannerApplication.App.Components.Pages.Dashboard
{
    public partial class P_Dashboard
    {
        private UserSessionModel _userSession = new();
        private DashboardRequestModel _reqModel = new();
        private DashboardResponseModel _resModel = new();
        protected override async Task OnInitializedAsync()
        {
            //await Notification(); 
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
                await Notification();
                await GetCourseList();
                StateHasChanged();
            }
        }

        async Task Notification()
        {
            NotificationRequestModel reqModel = new NotificationRequestModel
            {
                CurrentUserId = _userSession.UserId
            };
            var notiData = await _notificationService.GetAllNotification(reqModel);
            if (!notiData.Response.IsSuccess)
            {
                await _injectService.ErrorMessage(notiData.Response.Message);
                return;
            }

            _notificationStateContainer.NotificationCount = notiData.NotiList.Count;
        }

        async Task GetCourseList()
        {
            _reqModel.CurrentUserId = _userSession.UserId;
            _resModel = await _dashboardService.GetAllCourseList(_reqModel);
            if (!_resModel.Response.IsSuccess)
            {
                await _injectService.ErrorMessage(_resModel.Response.Message);
            }
        }

        private MudBlazor.Color GetStatus(string status)
        {
            return status switch
            {
                "Done" => Color.Success,
                "Pending" => Color.Warning,
                _ => Color.Default
            };
        }

        private string GetIcon(string status)
        {
            return status switch
            {
                "Done" => Icons.Material.Outlined.CheckCircleOutline,
                "Pending" => Icons.Material.Outlined.IncompleteCircle,
                _ => Icons.Material.Outlined.IncompleteCircle
            };
        }
    }
}
