namespace StudyPlannerApplication.App.Components.Pages.UserManagement.Authentication;

public partial class P_SignIn
{
    private SignInRequestModel _reqModel = new();
    private bool _isRegister = true;
    private bool isPasswordVisible = false;
    private bool isCPasswordVisible = false;
    private string password = "password";
    private string cPassword = "password";
    private string passwordIcon => isPasswordVisible ? "bi bi-eye-fill" : "bi bi-eye-slash-fill";
    private string cPasswordIcon => isCPasswordVisible ? "bi bi-eye-fill" : "bi bi-eye-slash-fill";
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            StateHasChanged();
        }
    }

    async Task SignIn()
    {
        if (!await CheckRequiredFields(_reqModel)) return;
        var model = await _signInService.SignIn(_reqModel);
        if (!model.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(model.Response.Message);
            return;
        }
        var userSessionModel = new UserSessionModel
        {
            UserName = model.UserName,
            Role = model.Role,
            UserId = model.UserId,
        };
        var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
        await customAuthStateProvider.UpdateAuthenticationState(userSessionModel);
        Navigation.NavigateTo("/dashboard");
    }
    async Task Register()
    {
        if (!await CheckRequiredFields(_reqModel)) return;
        var model = await _registerService.Register(_reqModel);
        if (!model.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(model.Response.Message);
            return;
        }
        await _injectService.SuccessMessage(model.Response.Message);
        _isRegister = false;
        _reqModel = new();
        StateHasChanged();
    }
    async Task<bool> CheckRequiredFields(SignInRequestModel _reqModel)
    {
        if (string.IsNullOrEmpty(_reqModel.UserName))
        {
            await _injectService.ErrorMessage("UserName Field is Required.");
            return false;
        }
        if (string.IsNullOrEmpty(_reqModel.Password))
        {
            await _injectService.ErrorMessage("Password Field is Required.");
            return false;
        }
        if (_isRegister)
        {
            if (string.IsNullOrEmpty(_reqModel.Email))
            {
                await _injectService.ErrorMessage("Email Field is Required.");
                return false;
            }
            if (string.IsNullOrEmpty(_reqModel.PhoneNo))
            {
                await _injectService.ErrorMessage("PhoneNo Field is Required.");
                return false;
            }
            if (_reqModel.Password!=_reqModel.ConfirmPassword)
            {
                await _injectService.ErrorMessage("Password and Confirm Password must be the same.");
                return false;
            }
        }
        return true;
    }

    private void ChangePage()
    {
        _isRegister = _isRegister ? false : true;
        _reqModel = new();
        StateHasChanged();
    }

    private void PasswordVisibility()
    {
        isPasswordVisible = isPasswordVisible ? false : true;
        password = password == "password" ? "text" : "password";
        StateHasChanged();
    }

    private void ConfirmPasswordVisibility()
    {
        isCPasswordVisible = isCPasswordVisible ? false : true;
        cPassword = cPassword == "password" ? "text" : "password";
        StateHasChanged();
    }
}
