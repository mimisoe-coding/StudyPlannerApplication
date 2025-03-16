using Microsoft.AspNetCore.Components.Forms;

namespace StudyPlannerApplication.App.Components.Pages.UserManagement.Profile;

public partial class P_Profile
{

    private UserSessionModel _userSession = new();
    private ProfileRequestModel _reqModel = new();
    private Result<ProfileResponseModel> _resModel = new();
    private string _imageBase64Str = string.Empty;
    private bool disabled = true;
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
        try
        {
            _resModel = await _profileService.Profile(_reqModel);
            if (!_resModel.Success)
            {
                await _injectService.ErrorMessage(_resModel.Message);
                return;
            }
            _imageBase64Str = "data:image;base64," + _resModel.Data.ImageStr;
            if(_resModel.Data.ImagePath is null)
            {
                _imageBase64Str="images/profile/profile.png";
            }
        }
        catch(Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public void Clear()
    {
        _reqModel.ImageUrl = null;
        _reqModel.ImageFile = null;
        _reqModel.ImageExtension = null;
        disabled = true;
        StateHasChanged();
    }

    public async Task Save()
    {
        try
        {
            if (string.IsNullOrEmpty(_reqModel.ImageUrl))
            {
                await _injectService.ErrorMessage("Please upload image.");
                return;
            }
            var result = await _profileService.Save(_reqModel);
            if (!result.Success)
            {
                await _injectService.ErrorMessage(result.Message);
            }
            disabled = true;
            await _injectService.SuccessMessage(result.Message);

        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    private async Task UploadFiles(IBrowserFile file)
    {
        try
        {
            if (file != null)
            {
                //if (file.Size > 5 * 1024 * 1024)
                //{
                //    await _injectService.ErrorMessage("File size exceeds the limit of 5 MB.");
                //    return;
                //}
                //var buffer = new byte[file.Size];
                //await file.OpenReadStream().ReadAsync(buffer);
                //_reqModel.ImageUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
                //_reqModel.ImageFile = Convert.ToBase64String(buffer);
                //if (!Check(_reqModel.ImageFile))
                //{
                //    await _injectService.ErrorMessage("File size exceeds the limit of 1 MB.");
                //    return;
                //}
                //_reqModel.ImageExtension = Path.GetExtension(file.Name);
                //StateHasChanged();

                MemoryStream ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                var bytes = ms.ToArray();
                string fileName = file.Name;
                var _base64Str = Convert.ToBase64String(bytes);
                _reqModel.ImageFile = _base64Str;
                _reqModel.ImageUrl = $"data:{file.ContentType};base64,{_base64Str}";
                disabled = false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }
}
