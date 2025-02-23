using StudyPlannerApplication.App.StateContainer;
using StudyPlannerApplication.Domain.Features.Notification;

namespace StudyPlannerApplication.App.Components.Pages.Dashboard
{
    public partial class P_Dashboard
    {
        private UserSessionModel _userSession = new();
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


    }
}
