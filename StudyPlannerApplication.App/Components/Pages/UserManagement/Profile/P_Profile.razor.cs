using StudyPlannerApplication.Domain.Features.UserManagement.Profile;

namespace StudyPlannerApplication.App.Components.Pages.UserManagement.Profile;

public partial class P_Profile
{
    private UserSessionModel _userSession = new();
    private ProfileRequestModel _reqModel = new();
    private ProfileResponseModel _resModel = new();
    private string _defaultProfileImage = "/images/profile/default-user.png";
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
            _reqModel.UserId = _userSession.UserId;
            await Profile();
            StateHasChanged();
        }
    }

    private async Task Profile()
    {
        _resModel = await _profileService.Profile(_reqModel);
        if (!_resModel.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(_resModel.Response.Message);
            return;
        }
    }
}
