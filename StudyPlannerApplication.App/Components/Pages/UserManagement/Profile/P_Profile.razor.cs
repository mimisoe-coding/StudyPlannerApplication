using Microsoft.AspNetCore.Components.Forms;
using System.Runtime.CompilerServices;

namespace StudyPlannerApplication.App.Components.Pages.UserManagement.Profile;

public partial class P_Profile
{
    private UserSessionModel _userSession = new();
    private ProfileRequestModel _reqModel = new();
    private Result<ProfileResponseModel> _resModel = new();
    private string _imageBase64Str = string.Empty;
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
        //_reqModel.ImageUrl = "images/profile/mony.jpg";
        _resModel = await _profileService.Profile(_reqModel);
        if (!_resModel.Success)
        {
            await _injectService.ErrorMessage(_resModel.Message);
            return;
        }
        _resModel.Data.ImagePath = _resModel.Data.ImagePath ?? "images/profile/profile.png";
        _imageBase64Str = "data:image;base64," + _resModel.Data.ImageStr;
    }

    public async Task Clear()
    {
        _reqModel.ImageUrl = null;
        _reqModel.ImageFile = null;
        _reqModel.ImageExtension = null;
        StateHasChanged();
    }

    public async Task Save()
    {
        var result = await _profileService.Save(_reqModel);
        if (!result.Success)
        {
            await _injectService.ErrorMessage(result.Message);
        }
        await _injectService.SuccessMessage(result.Message);    
    }

    private async Task UploadFiles(IBrowserFile file)
    {
        if (file != null)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            _reqModel.ImageUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
            _reqModel.ImageFile = Convert.ToBase64String(buffer);
            _reqModel.ImageExtension = Path.GetExtension(file.Name);
        }
    }
}
