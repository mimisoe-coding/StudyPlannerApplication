using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudyPlannerApplication.Domain.Features.UserManagement.Login;

namespace StudyPlannerApplication.App.Components.Pages.UserManagement.Authentication
{
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
            _reqModel.UserName = "MiMiSoe";
            _reqModel.Password = "12345";
            var model = await _logInService.LogIn(_reqModel);
            if(model is not null)
            {
                Navigation.NavigateTo("/dd");
            }
        }
    }
}
