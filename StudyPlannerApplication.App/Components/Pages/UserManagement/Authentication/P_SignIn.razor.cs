namespace StudyPlannerApplication.App.Components.Pages.UserManagement.Authentication;

public partial class P_SignIn
{
    private LogInRequestModel _reqModel = new();
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

    async Task LogIn()
    {
        if (!await CheckRequiredFields(_reqModel)) return;
        var model = await _logInService.LogIn(_reqModel);
        if (!model.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(model.Response.Message);
            //bool success = await _injectService.ConfirmMessageBox(model.Response.Message);
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
        Navigation.NavigateTo("/dd");
    }
    async Task Register()
    {

    }
    async Task<bool> CheckRequiredFields(LogInRequestModel _reqModel)
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
