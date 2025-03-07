using Radzen.Blazor;
using Radzen;
using StudyPlannerApplication.Domain.Features.Exam;
using StudyPlannerApplication.Domain.Features.Reminder;

namespace StudyPlannerApplication.App.Components.Pages.Reminder
{
    public partial class P_Reminder
    {
        private UserSessionModel _userSession = new();
        private ReminderRequestModel _reqModel = new();
        private ReminderResponseModel _resModel = new();
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
                await List();
                StateHasChanged();
            }
        }

        async Task List()
        {
            _reqModel.CurrentUserId = _userSession.UserId;
            _resModel = await _reminderService.GetAllCourse(_reqModel);
            if (!_resModel.Response.IsSuccess)
            {
                await _injectService.ErrorMessage(_resModel.Response.Message);
                return;
            }
        }
    }
}
