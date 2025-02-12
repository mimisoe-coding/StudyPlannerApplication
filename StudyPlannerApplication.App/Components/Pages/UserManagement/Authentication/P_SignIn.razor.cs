namespace StudyPlannerApplication.App.Components.Pages.UserManagement.Authentication;

public partial class P_SignIn
{
    private LogInRequestModel _reqModel = new();
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            StateHasChanged();
        }
    }

    async Task LogIn()
    {
        var model = await _logInService.LogIn(_reqModel);
        if (!model.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(model.Response.Message);
            //bool success = await _injectService.ConfirmMessageBox(model.Response.Message);
            return;
        }
        Navigation.NavigateTo("/dd");
    }
}
